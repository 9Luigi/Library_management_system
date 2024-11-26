using Library.Controllers;
using Library.Models;
using Library.Services.Repository;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Library
{
	public partial class FormMembers : Form
	{
		public FormMembers()
		{
			InitializeComponent();
			CancellationTokenSource = new CancellationTokenSource();
			FlendOrRecieveBook = new FormBorrowOrRecieveBook();
			FaddEdit_prop = new FaddEdit_prop();
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
		private async void AddMemberToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//Transfer data to FaddEdit_prop form, subscribed to MemberCreateOrUpdateEvent on FaddEdit_prop constructor
			MemberCreateOrUpdateEvent!.Invoke(new MemberEventArgs("CREATE"));
			FaddEdit_prop.ShowDialog();
			await RefreshDataGridForMembers();
		}
		private async void FMembers_Load(object sender, EventArgs e)
		{
			await RefreshDataGridForMembers();
		}
		private async void EditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(bool b, long i) = DataGridViewController.TryGetIINFromRow(dataGridViewForMembers);
			if (b)
			{
				MemberCreateOrUpdateEvent!.Invoke(new MemberEventArgs("EDIT", i));
				FaddEdit_prop.ShowDialog();
				await RefreshDataGridForMembers();
			}
		}

		/// <summary>
		/// Handles the click event of the "Delete" menu item, removing a member from the database.
		/// </summary>
		/// <param name="sender">The object that triggered the event (typically the menu item itself).</param>
		/// <param name="e">The event arguments containing information about the click event.</param>
		private async void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Retrieve data from the selected row in the DataGridView
			(bool b, long i) = DataGridViewController.TryGetIINFromRow(dataGridViewForMembers);

			// If IIN was successfully extracted
			if (b)
			{
				// Open the database context asynchronously
				await using LibraryContextForEFcore db = new();

				// Search for the member by IIN
				Member? memberToDelete = await db.Members.FirstOrDefaultAsync(m => m.IIN == i);

				// If the member is not found, display an error message
				if (memberToDelete == null)
				{
					MessageBox.Show($"Cannot find member with IIN: {i}");
					return;
				}

				// Show a confirmation dialog to the user
				DialogResult result = MessageBox.Show("Are you sure to remove?", "Removing member", MessageBoxButtons.YesNo);

				// If the user confirms the deletion
				if (result == DialogResult.Yes)
				{
					// Remove the member from the database
					db.Members.Remove(memberToDelete);

					// Save changes to the database and check if any rows were affected
					if (await db.SaveChangesAsync() > 0)
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

		private void View_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex == 0)
			{
				dataGridViewForMembers.CurrentCell = dataGridViewForMembers.Rows[e.RowIndex].Cells[e.ColumnIndex];
				Point relativeCursorPosition = dataGridViewForMembers.PointToClient(Cursor.Position);
				cmMember.Show(dataGridViewForMembers, relativeCursorPosition);
				//if clicked by right button cell is IIN then popup menu executes
			}
		}
		private void TbIINSearch_TextChanged(object sender, EventArgs e)
		{
			/*using LibraryContextForEFcore db = new();
			if (TbIINSearch.Text.Length > 3)
			{
				_ = long.TryParse(TbIINSearch.Text, out long IIN);
				if (IIN != 0)
				{
					var MatchedMembers = db.Members.Where(m => EF.Functions.Like(m.IIN.ToString(), $"%{IIN}%")).
						Select(m => new { m.IIN, m.Name, m.Surname, m.Age }).ToList();
					dataGridViewForMembers.DataSource = MatchedMembers; //TODO something //???
				}
			}
			else
			{
				var users = db.Members.Select(m => new
				{
					m.IIN,
					m.Name,
					m.Surname,
					m.Age
				}).ToList();
				dataGridViewForMembers.DataSource = users;
			}*/
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
				using (LibraryContextForEFcore db = new())
				{
					// Get the total count of members
					int totalMembersCount = await GetTotalMembersCountAsync(db);

					// Check if the operation was cancelled
					if (CancellationToken.IsCancellationRequested) return;

					// Update the progress bar as data is being loaded
					await UpdateProgressBarAsync(0, 25);

					// Retrieve members from the database
					var users = await GetMembersFromDbAsync(db);

					// Check if the operation was cancelled
					if (CancellationToken.IsCancellationRequested) return;

					// Update the progress bar after retrieving data
					await UpdateProgressBarAsync(25, 50);

					// Update the DataGridView with the retrieved data
					await UpdateDataGridViewAsync(users);

					// Check if the operation was cancelled
					if (CancellationToken.IsCancellationRequested) return;

					// Final progress bar update
					await UpdateProgressBarAsync(50, 100);
				}

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

		private async Task<int> GetTotalMembersCountAsync(LibraryContextForEFcore db)
		{
			return await Task.Run(() => db.Members.Count());
		}

		private async Task<List<dynamic>> GetMembersFromDbAsync(LibraryContextForEFcore db)
		{
			var members = await db.Members.Include(m => m.Books)
				.Select(m => new
				{
					m.IIN,
					m.Name,
					m.Surname,
					m.Age,
					m.RegistrationDate,
					Books = string.Join(", ", m.Books.Select(b => b.Title)),
				})
				.OrderByDescending(m => m.RegistrationDate)
				.ToListAsync();

			return members.Cast<dynamic>().ToList();
		}

		private async Task UpdateProgressBarAsync(int minValue, int maxValue)
		{
			await ProgressBarController.pbProgressCgange(this, pbMembers, minValue, maxValue);
		}

		private async Task UpdateDataGridViewAsync(List<dynamic> users)
		{
			this.Invoke(new Action(() =>
			{
				dataGridViewForMembers.DataSource = users;
				DataGridViewController.CustomizeDataGridView(dataGridViewForMembers);
			}));
		}

		private async Task ResetProgressBarAsync()
		{
			this.Invoke(ProgressBarController.pbProgressReset, pbMembers);
		}

		private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		private void FMembers_FormClosing(object sender, FormClosingEventArgs e)
		{
			CancellationTokenSource!.Cancel(); //most likely bad arhitecture but allow to avoid error(this.invoke after this closed)
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
		private async void SeeLendedBooksForThisMemberToolStripMenuItem_Click(object sender, EventArgs e)
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
