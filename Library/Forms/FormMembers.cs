using Library.Controllers;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Library
{
	public partial class FormMembers : Form
	{
		Repository<Member> _memberRepository;
		public FormMembers()
		{
			InitializeComponent();
			CancellationTokenSource = new CancellationTokenSource();
			FlendOrRecieveBook = new FormBorrowOrRecieveBook();
			FaddEdit_prop = new FaddEdit_prop();
			var dbContext = new LibraryContextForEFcore();
			_memberRepository = new(dbContext);
		}
		internal class MemberEventArgs : EventArgs //for transfer IIN and Action to other forms via event
		{
			internal long IIN { get; private set; }
			internal string Action { get; private set; }
			public MemberEventArgs(string action, long IIN = 0)
			{
				this.IIN = IIN;
				Action = action;
			}
		}
		internal FormBorrowOrRecieveBook FlendOrRecieveBook { get; private set; }
		internal FaddEdit_prop FaddEdit_prop { get; private set; }
		internal long IIN { get; set; }
		internal CancellationTokenSource CancellationTokenSource { get; set; }
		internal CancellationToken CancellationToken { get; set; }
		internal delegate void MemberCreateOrUpdateDelegate(MemberEventArgs e);
		static internal event MemberCreateOrUpdateDelegate? MemberCreateOrUpdateEvent;

		readonly ControlsController controlsController = new();
		/// <summary>
		/// Handles the click event for the "Add Member" menu item in the context menu.
		/// Triggers the "CREATE" action, opens the member creation form, and refreshes the data grid.
		/// </summary>
		private async void AddMemberToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				// Transfer data to the FaddEdit_prop form by invoking the MemberCreateOrUpdateEvent.
				// The FaddEdit_prop form is subscribed to this event in its constructor.
				MemberCreateOrUpdateEvent!.Invoke(new MemberEventArgs("CREATE"));

				// Show the form dialog for adding a new member
				FaddEdit_prop.ShowDialog();

				// Refresh the data grid asynchronously after the form is closed
				await RefreshDataGridForMembers();
			}
			catch (Exception ex)
			{
				// Handle any exceptions that may occur during the process
				MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async void FMembers_Load(object sender, EventArgs e)
		{
			await RefreshDataGridForMembers();
		}
		/// <summary>
		/// Handles the click event for the "Edit" menu item in the context menu.
		/// If the row contains a valid IIN, triggers the edit action and refreshes the data grid.
		/// </summary>
		private async void EditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Attempt to retrieve a valid IIN from the selected row in the DataGridView
			(bool isValid, long IIN) = DataGridViewController.TryGetIINFromRow(dataGridViewForMembers);

			if (isValid)
			{
				// Trigger the edit event with the IIN as an argument
				MemberCreateOrUpdateEvent!.Invoke(new MemberEventArgs("EDIT", IIN));

				// Show the edit form dialog
				FaddEdit_prop.ShowDialog();

				// Refresh the data grid asynchronously
				await RefreshDataGridForMembers();
			}
		}


		/// <summary>
		/// Handles the click event of the "Delete" menu item, removing a member from the database.
		/// </summary>
		private async void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Retrieve data from the selected row in the DataGridView
			(bool IsValid, long IIN) = DataGridViewController.TryGetIINFromRow(dataGridViewForMembers);

			// If IIN was successfully checked 
			if (IsValid)
			{
				await using LibraryContextForEFcore db = new();
				var repos = new Repository<Member>(db);
				// Show a confirmation dialog to the user
				DialogResult result = MessageBox.Show("Are you sure to remove?", "Removing member", MessageBoxButtons.YesNo);

				// If the user confirms the deletion
				if (result == DialogResult.Yes)
				{

					// Remove the member from the database
					if (await repos.DeleteAsync(IIN) == true)
					{
						// Inform the user that the member was successfully removed
						MessageBox.Show("Member successfully removed");
						// Refresh the DataGridView with updated data
						await RefreshDataGridForMembers();
					}
				}
			}
			else
			{
				// If the IIN could not be extracted from the DataGridView row
				MessageBox.Show("Cannot delete this member, try later or contact your system administrator");
			}
		}

		/// <summary>
		/// Handles the cell mouse click event for the DataGridView.
		/// Displays the context menu when right-clicking on any cell, excluding the headers.
		/// </summary>
		private void View_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			// Check if the right mouse button was clicked and the click is on a valid cell (not a header)
			if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
			{
				// Set the current cell to the one that was clicked
				dataGridViewForMembers.CurrentCell = dataGridViewForMembers.Rows[e.RowIndex].Cells[e.ColumnIndex];

				// Get the cursor position relative to the DataGridView
				Point relativeCursorPosition = dataGridViewForMembers.PointToClient(Cursor.Position);

				// Show the context menu at the cursor position
				cmMember.Show(dataGridViewForMembers, relativeCursorPosition);
			}
		}

		private async void TbIINSearch_TextChanged(object sender, EventArgs e)
		{
			if (TbIINSearch.Text.Length > 3)
			{
				if (long.TryParse(TbIINSearch.Text, out long IIN) && IIN != 0)
				{
					var matchedMembers = await _memberRepository.GetWithProjectionAsync( //TODO create class or structure for projected Member
						m => new
						{
							m.IIN,
							m.Name,
							m.Surname,
							m.Age,
							m.RegistrationDate,
							Books = string.Join(", ", m.Books.Select(b => b.Title))
						},
						Convert.ToInt64(TbIINSearch.Text),
						m => m.IIN,
						m => m.Books
					);

					dataGridViewForMembers.DataSource = matchedMembers;
				}
				else
				{
					// Если введен некорректный IIN, можно очистить DataGrid или показать сообщение
					dataGridViewForMembers.DataSource = null; // Очистить DataGrid
				}
			}
			else
			{
				// Если введено слишком короткое значение, загружаем всех членов
				var members = await _memberRepository.GetWithProjectionAsync(
					m => new
					{
						m.IIN,
						m.Name,
						m.Surname,
						m.Age,
						m.RegistrationDate,
						Books = string.Join(", ", m.Books.Select(b => b.Title))
					},
					m => m.Books
				);

				dataGridViewForMembers.DataSource = members;
			}
		}

		/// <summary>
		/// Refreshes the DataGrid with member data from the database asynchronously.
		/// Disables controls while data is loading and updates progress as data is being fetched.
		/// </summary>
		private async Task RefreshDataGridForMembers()
		{
			// Disable controls while data from DB is loading
			await SetControlsEnabledAsync(false);

			CancellationToken = CancellationTokenSource.Token;

			Task fillGridWithAllMembers = new TaskFactory().StartNew(new Action(async () =>
			{
				// Get the total count of members
				int totalMembersCount = await GetTotalMembersCountAsync();

				// Check if the operation was cancelled
				if (CancellationToken.IsCancellationRequested) return;

				// Update the progress bar as data is being loaded
				await UpdateProgressBarAsync(0, 25);

				// Retrieve members from the database
				var users = await GetMembersFromDbAsync();

				// Check if the operation was cancelled
				if (CancellationToken.IsCancellationRequested) return;

				// Update the progress bar after retrieving data
				await UpdateProgressBarAsync(25, 50);

				// Update the DataGridView with the retrieved data
				UpdateDataGridViewAsync(users);

				// Check if the operation was cancelled
				if (CancellationToken.IsCancellationRequested) return;

				// Final progress bar update
				await UpdateProgressBarAsync(50, 100);
				// Sleep for a short period before resetting progress
				Thread.Sleep(500);

				// Check if the operation was cancelled
				if (CancellationToken.IsCancellationRequested) return;

				// If the progress bar is disposed, do not continue
				if (pbMembers.IsDisposed) return;

				// Reset the progress bar and enable controls after data is loaded
				await ResetProgressBarAsync();
				await SetControlsEnabledAsync(true);

			}), CancellationToken);
		}


		private async Task SetControlsEnabledAsync(bool isEnabled)
		{
			await controlsController.SetControlsEnableFlag(this, this.Controls, isEnabled);
		}

		/// <summary>
		/// Asynchronously retrieves the total count of members from the database.
		/// </summary>
		/// <returns>The total number of members in the database.</returns>
		private async Task<int> GetTotalMembersCountAsync()
		{
			// Use Task.Run to execute the database query asynchronously on a separate thread
			return await Task.Run(() => _memberRepository._dbContext.Members.Count());
		}


		private async Task<List<dynamic>> GetMembersFromDbAsync()
		{

			var members = await _memberRepository.GetWithProjectionAsync(
				m => new
				{
					m.IIN,
					m.Name,
					m.Surname,
					m.Age,
					m.RegistrationDate,
					Books = string.Join(", ", m.Books.Select(b => b.Title))
				},
				m => m.Books
			);
			return members.Cast<dynamic>().ToList();
		}


		/// <summary>
		/// Asynchronously updates the progress bar with the specified minimum and maximum values.
		/// </summary>
		/// <param name="minValue">The minimum value of the progress bar.</param>
		/// <param name="maxValue">The maximum value of the progress bar.</param>
		private async Task UpdateProgressBarAsync(int minValue, int maxValue)
		{
			// Call the ProgressBarController to update the progress bar asynchronously
			await ProgressBarController.pbProgressCgange(this, pbMembers, minValue, maxValue);
		}

		/// <summary>
		/// Asynchronously updates the DataGridView with a list of users.
		/// Invokes the necessary UI updates on the main thread to modify the DataGridView's DataSource and customize it.
		/// </summary>
		/// <param name="users">The list of users to display in the DataGridView.</param>
		private void UpdateDataGridViewAsync(List<dynamic> users)
		{
			// Invoke the update on the UI thread to modify the DataGridView safely
			this.Invoke(new Action(() =>
			{
				dataGridViewForMembers.DataSource = users; // Set the new data source for the grid
				DataGridViewController.CustomizeDataGridView(dataGridViewForMembers); // Customize the grid appearance
			}));
		}

		/// <summary>
		/// Asynchronously resets the progress bar to its initial state.
		/// </summary>
		private async Task ResetProgressBarAsync()
		{
			if (pbMembers.IsDisposed) return;
			await Task.Run(() =>
			{
				this.BeginInvoke(async () =>
				{
					await ProgressBarController.pbProgressReset(pbMembers);
				});
			});
		}

		/// <summary>
		/// Handles the click event for the "Exit" menu item. Closes the form.
		/// </summary>
		private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Close the current form
			this.Close();
		}

		private void FMembers_FormClosing(object sender, FormClosingEventArgs e)
		{
			CancellationTokenSource!.Cancel(); //TODO most likely bad arhitecture but allow to avoid error(this.invoke after this closed)
		}
		/// <summary>
		/// Clears IIN textbox when clicked on it
		/// </summary>
		private void TbIINSearch_Click(object sender, EventArgs e)
		{
			TbIINSearch.Text = "";
		}
		/// <summary>
		/// Handles the click event of the "Le" menu item, which processes the borrowing action for a selected member.
		/// If the selected member has a valid IIN, it invokes an event to initiate the borrowing process and shows the borrowing form.
		/// </summary>
		private async void LeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(bool b, long i) = DataGridViewController.TryGetIINFromRow(dataGridViewForMembers);
			if (b)
			{
				// If a valid member is selected, invoke the borrowing event
				MemberCreateOrUpdateEvent!.Invoke(new MemberEventArgs("BORROW", i));

				// Display the borrowing form as a dialog
				FlendOrRecieveBook.ShowDialog();

				// Refresh the data grid to reflect any changes made after borrowing
				await RefreshDataGridForMembers();
			}
			else
			{
				// If no valid member is selected, show an error message
				MessageBox.Show("Please select a member to proceed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// Handles the click event of the "See Lended Books for This Member" menu item.
		/// Checks if a member has borrowed any books and, if so, displays their information in another form.
		/// </summary>
		private async void SeeLendedBooksForThisMemberToolStripMenuItem_Click(object sender, EventArgs e) //TODO use Repository
		{
			// Check if a valid member is selected and retrieve their IIN
			(bool b, long i) = DataGridViewController.TryGetIINFromRow(dataGridViewForMembers);

			if (b)
			{
				try
				{
					using LibraryContextForEFcore db = new();

					// Combine the member details and borrowed books into one query to optimize DB calls
					var memberData = await db.Members
						.Where(m => m.IIN == i)
						.Select(m => new
						{
							m.Name,
							m.Surname,
							Books = m.Books.Select(b => new
							{
								b.Id,
								b.Title,
								b.Genre
							}).ToList()
						})
						.FirstOrDefaultAsync();

					if (memberData == null)
					{
						MessageBox.Show($"Member with IIN: {i} not found.");
						return;
					}

					// Check if the member has borrowed books
					if (memberData.Books.Any())
					{
						// If the member has borrowed books, trigger the event and show the form
						MemberCreateOrUpdateEvent!.Invoke(new MemberEventArgs("RETURN", i));
						FlendOrRecieveBook.ShowDialog();

						// Refresh the data grid after the operation
						await RefreshDataGridForMembers();
					}
					else
					{
						MessageBox.Show($"{memberData.Name} {memberData.Surname} has not borrowed any books yet.");
					}
				}
				catch (Exception ex)
				{
					// Handle any exceptions that might occur during database access or other operations
					MessageBox.Show($"An error occurred: {ex.Message}");
				}
			}
		}

	}
}
