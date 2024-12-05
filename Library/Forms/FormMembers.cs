using Library.Controllers;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Library
{
	public partial class FormMembers : Form
	{
		#region FieldsAndProperties
		readonly Repository<Member> _memberRepository;
		readonly ILogger _logger;
		internal FormBorrowOrRecieveBook FlendOrRecieveBook { get; private set; }
		internal FaddEdit_prop FaddEdit_prop { get; private set; }
		internal CancellationTokenSource CancellationTokenSource { get; set; }
		internal CancellationToken CancellationToken { get; set; }
		internal delegate void MemberCreateOrUpdateDelegate(MemberEventArgs e);
		static internal event MemberCreateOrUpdateDelegate? MemberCreateOrUpdateEvent;
		readonly ControlsController controlsController = new();
		public FormMembers()
		{
			InitializeComponent();
			CancellationTokenSource = new CancellationTokenSource();
			FlendOrRecieveBook = new FormBorrowOrRecieveBook();
			FaddEdit_prop = new FaddEdit_prop();
			var _dbContext = new LibraryContextForEFcore();
			_logger = LoggerService.CreateLogger<Repository<Member>>();
			_memberRepository = new(_dbContext);
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
		#endregion
		//TODO Create Repositoty<Member> class and transfer all repository method to that
		/// <summary>
		/// Handles the click event for the "Add Member" menu item in the context menu.
		/// Triggers the "CREATE" action, opens the member creation form, and refreshes the data grid.
		/// </summary>
		/// <summary>
		/// Handles the click event for the "Add Member" menu item in the context menu.
		/// Triggers the "CREATE" action, opens the member creation form, and refreshes the data grid.
		/// </summary>
		private async void AddMemberToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_logger.LogInformation("AddMemberToolStripMenuItem_Click: Started processing the event.");
			try
			{
				// Trigger the "CREATE" action by invoking the MemberCreateOrUpdateEvent.
				MemberCreateOrUpdateEvent?.Invoke(new MemberEventArgs("CREATE"));
				_logger.LogInformation("MemberCreateOrUpdateEvent was successfully invoked.");

				// Show the form dialog for adding a new member.
				FaddEdit_prop.ShowDialog();
				_logger.LogInformation("Member creation form was displayed successfully.");

				// Refresh the data grid asynchronously after the form is closed.
				await RefreshDataGridForMembers();
				_logger.LogInformation("Data grid for members was refreshed.");
			}
			catch (Exception ex)
			{
				// Log the error with exception details.
				_logger.LogError(ex, "An error occurred while handling AddMemberToolStripMenuItem_Click");

				// Show an error message to the user.
				MessageBox.Show(
					$"An unexpected error occurred: {ex.Message}\nPlease contact support if the problem persists.",
					"Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error
				);
			}
		}

		private async void FMembers_Load(object sender, EventArgs e)
		{
			_logger.LogInformation("FMembers_Load: Started processing the event.");
			_logger.LogInformation("Entered to FMembers_Load method");
			await RefreshDataGridForMembers();
			_logger.LogInformation("FMembers_Load were loaded");
		}
		/// <summary>
		/// Handles the click event for the "Edit" menu item in the context menu.
		/// If the row contains a valid IIN, triggers the edit action and refreshes the data grid.
		/// </summary>
		private async void EditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_logger.LogInformation("EditToolStripMenuItem_Click: Started processing the event.");
			// Attempt to retrieve a valid IIN from the selected row in the DataGridView
			(bool isValid, long IIN) = DataGridViewController.TryGetIINFromRow(dataGridViewForMembers);

			if (isValid)
			{

				// Trigger the edit event with the IIN as an argument
				MemberCreateOrUpdateEvent!.Invoke(new MemberEventArgs("EDIT", IIN));
				_logger.LogInformation("MemberCreateOrUpdateEvent invoked");
				_logger.LogInformation("Start to try for FaddEdit_prop.ShowDialog()");
				// Show the edit form dialog
				FaddEdit_prop.ShowDialog();
				_logger.LogInformation("FaddEdit_prop form were closed");
				// Refresh the data grid asynchronously
				await RefreshDataGridForMembers();
			}
		}
		/// <summary>
		/// Handles the click event of the "Delete" menu item, removing a member from the database.
		/// </summary>
		private async void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_logger.LogInformation("DeleteToolStripMenuItem_Click: Started processing the event.");

			// Retrieve data from the selected row in the DataGridView
			(bool IsValid, long IIN) = DataGridViewController.TryGetIINFromRow(dataGridViewForMembers);
			_logger.LogInformation("Attempted to extract IIN from the selected DataGridView row.");

			// If IIN was successfully checked
			if (IsValid)
			{
				_logger.LogInformation($"IIN successfully extracted: {IIN}.");

				// Show a confirmation dialog to the user
				_logger.LogInformation("Displaying confirmation dialog for deletion.");
				DialogResult result = MessageBox.Show("Are you sure to remove?", "Removing member", MessageBoxButtons.YesNo);

				// If the user confirms the deletion
				if (result == DialogResult.Yes)
				{
					_logger.LogInformation("User confirmed deletion with YES.");

					// Remove the member from the database
					if (await _memberRepository.DeleteAsync(IIN) == true)
					{
						_logger.LogInformation("Member successfully removed from the database.");

						// Inform the user that the member was successfully removed
						MessageBox.Show("Member successfully removed");

						// Refresh the DataGridView with updated data
						await RefreshDataGridForMembers();
						_logger.LogInformation("Data grid for members refreshed with updated data.");
					}
				}
			}
			else
			{
				_logger.LogWarning("Failed to extract IIN from the DataGridView row.");
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
			_logger.LogInformation("Entered View_CellMouseClick method");

			// Check if the right mouse button was clicked and the click is on a valid cell (not a header)
			if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
			{
				_logger.LogInformation("Right mouse button clicked on valid cell at row {RowIndex}, column {ColumnIndex}", e.RowIndex, e.ColumnIndex);

				// Set the current cell to the one that was clicked
				dataGridViewForMembers.CurrentCell = dataGridViewForMembers.Rows[e.RowIndex].Cells[e.ColumnIndex];

				// Get the cursor position relative to the DataGridView
				Point relativeCursorPosition = dataGridViewForMembers.PointToClient(Cursor.Position);
				_logger.LogInformation("Cursor position relative to DataGridView: {CursorPosition}", relativeCursorPosition);

				// Show the context menu at the cursor position
				cmMember.Show(dataGridViewForMembers, relativeCursorPosition);
				_logger.LogInformation("Context menu displayed at position {CursorPosition}", relativeCursorPosition);
			}
			else
			{
				_logger.LogWarning("Right mouse button was clicked, but not on a valid cell (row: {RowIndex}, column: {ColumnIndex})", e.RowIndex, e.ColumnIndex);
			}
		}


		private async void TbIINSearch_TextChanged(object sender, EventArgs e)
		{
			_logger.LogInformation("Entered TbIINSearch_TextChanged method. Text length: {TextLength}", TbIINSearch.Text.Length);

			try
			{
				if (TbIINSearch.Text.Length > 3)
				{
					string input = TbIINSearch.Text.Trim();
					_logger.LogInformation("User entered input: {Input}", input);

					if (long.TryParse(input, out long IIN) && IIN != 0)
					{
						_logger.LogInformation("Valid IIN parsed: {IIN}", IIN);

						// Perform the member search
						var matchedMembers = await _memberRepository.GetWithProjectionAsync(
							m => new
							{
								m.IIN,
								m.Name,
								m.Surname,
								m.Age,
								m.RegistrationDate,
								Books = string.Join(", ", m.Books.Select(b => b.Title))
							},
							IIN,
							m => m.IIN,
							m => m.Books
						);

						dataGridViewForMembers.DataSource = matchedMembers;
						_logger.LogInformation("Matched members found and displayed in DataGridView.");
					}
					else
					{
						dataGridViewForMembers.DataSource = null;
						_logger.LogWarning("Invalid IIN input: {Input}", input);
					}
				}
				else
				{
					// Load all members when the input is shorter than 3 characters
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
					_logger.LogInformation("All members loaded and displayed in DataGridView.");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while searching members.");
				MessageBox.Show("An error occurred while fetching the data. Please try again later.");
			}
		}


		/// <summary>
		/// Refreshes the DataGrid with member data from the database asynchronously.
		/// Disables controls while data is loading and updates progress as data is being fetched.
		/// </summary>
		private async Task RefreshDataGridForMembers()
		{
			_logger.LogInformation("Entered RefreshDataGridForMembers method.");

			try
			{
				// Disable controls while data from DB is loading
				await SetControlsEnabledAsync(false);
				_logger.LogInformation("Controls disabled while loading data.");

				CancellationToken = CancellationTokenSource.Token;

				// Use Task.Run to start the task without manually calling Start()
				await Task.Run(async () =>
				{
					try
					{
						// Get the total count of members
						int totalMembersCount = await GetTotalMembersCountAsync();
						_logger.LogInformation("Total members count retrieved: {TotalCount}", totalMembersCount);

						// Check if the operation was cancelled
						if (CancellationToken.IsCancellationRequested) return;

						// Update the progress bar as data is being loaded
						await UpdateProgressBarAsync(0, 25);
						_logger.LogInformation("Progress bar updated to 25%.");

						// Retrieve members from the database
						var users = await GetMembersFromDbAsync();
						_logger.LogInformation("Members data retrieved from database.");

						// Check if the operation was cancelled
						if (CancellationToken.IsCancellationRequested) return;

						// Update the progress bar after retrieving data
						await UpdateProgressBarAsync(25, 50);
						_logger.LogInformation("Progress bar updated to 50%.");

						// Update the DataGridView with the retrieved data
						UpdateDataGridViewAsync(users);
						_logger.LogInformation("DataGridView updated with retrieved members.");

						// Check if the operation was cancelled
						if (CancellationToken.IsCancellationRequested) return;

						// Final progress bar update
						await UpdateProgressBarAsync(50, 100);
						_logger.LogInformation("Progress bar updated to 100%.");

						// Sleep for a short period before resetting progress
						Thread.Sleep(500);

						// Check if the operation was cancelled
						if (CancellationToken.IsCancellationRequested) return;

						// If the progress bar is disposed, do not continue
						if (pbMembers.IsDisposed) return;

						// Reset the progress bar and enable controls after data is loaded
						await ResetProgressBarAsync();
						await SetControlsEnabledAsync(true);
						_logger.LogInformation("Progress bar reset, and controls re-enabled.");
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "An error occurred while loading members.");
						MessageBox.Show("An error occurred while loading members. Please try again later.");
					}
				}, CancellationToken);

				_logger.LogInformation("Member loading task started.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred in RefreshDataGridForMembers method.");
				MessageBox.Show($"An error with message:{ex.Message}, occurred while preparing data for display. Please try again later.");
			}
		}




		private async Task SetControlsEnabledAsync(bool isEnabled)
		{
			_logger.LogInformation("SetControlsEnabledAsync started. Controls will be {Status}.", isEnabled ? "enabled" : "disabled");

			// Set controls enabled or disabled
			await controlsController.SetControlsEnableFlag(this, this.Controls, isEnabled);

			_logger.LogInformation("Controls successfully {Status}.", isEnabled ? "enabled" : "disabled");
		}

		/// <summary>
		/// Asynchronously retrieves the total count of members from the database.
		/// </summary>
		/// <returns>The total number of members in the database.</returns>
		private async Task<int> GetTotalMembersCountAsync()
		{
			try
			{
				_logger.LogInformation("GetTotalMembersCountAsync started.");

				// Execute the database query asynchronously
				int totalMembersCount = await Task.Run(() => _memberRepository._dbContext.Members.Count());

				_logger.LogInformation("Total members count retrieved: {Count}", totalMembersCount);

				return totalMembersCount;
			}
			catch (Exception ex)
			{
				// Log any error that occurs during the database query
				_logger.LogError(ex, "An error occurred while retrieving the total members count.");
				MessageBox.Show("An error occurred while retrieving the total members count. Please try again.");
				return 0; // Return a default value in case of an error
			}
		}



		/// <summary>
		/// Asynchronously retrieves a list of members from the database with specific projections (IIN, Name, Surname, Age, Registration Date, and Books).
		/// </summary>
		/// <returns>A list of dynamic objects representing the members retrieved from the database. If an error occurs, returns an empty list.</returns>
		/// <exception cref="Exception">Thrown when an error occurs while querying the database.</exception>
		private async Task<List<dynamic>> GetMembersFromDbAsync()
		{
			try
			{
				_logger.LogInformation("GetMembersFromDbAsync started.");

				// Retrieve members with projection from the database
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

				// Log the number of members retrieved
				_logger.LogInformation("Retrieved {Count} members from the database.", members.Count());

				// Return the members as a list of dynamic objects
				return members.Cast<dynamic>().ToList();
			}
			catch (Exception ex)
			{
				// Log any errors that occur during the database operation
				_logger.LogError(ex, "An error occurred while retrieving members from the database.");
				MessageBox.Show("An error occurred while retrieving the members. Please try again.");
				return new List<dynamic>(); // Return an empty list in case of error
			}
		}




		/// <summary>
		/// Asynchronously updates the progress bar's value from a minimum value to a maximum value.
		/// </summary>
		/// <param name="minValue">The starting value for the progress bar.</param>
		/// <param name="maxValue">The ending value for the progress bar.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown if the minValue or maxValue is invalid (e.g., maxValue < minValue).</exception>
		private async Task UpdateProgressBarAsync(int minValue, int maxValue)
		{
			try
			{
				_logger.LogInformation("Updating progress bar from {MinValue} to {MaxValue}.", minValue, maxValue);

				// Call the ProgressBarController to update the progress bar asynchronously
				await ProgressBarController.pbProgressCgange(this, pbMembers, minValue, maxValue);

				_logger.LogInformation("Progress bar updated successfully.");
			}
			catch (ArgumentOutOfRangeException ex)
			{
				// Log any ArgumentOutOfRangeException errors (if maxValue is less than minValue)
				_logger.LogError(ex, "Invalid progress bar range: minValue = {MinValue}, maxValue = {MaxValue}.", minValue, maxValue);
				MessageBox.Show("Error: Invalid range for the progress bar.");
			}
			catch (Exception ex)
			{
				// Log any other errors
				_logger.LogError(ex, "An error occurred while updating the progress bar.");
				MessageBox.Show("An unexpected error occurred while updating the progress bar.");
			}
		}


		/// <summary>
		/// Asynchronously updates the DataGridView with a list of users.
		/// Invokes the necessary UI updates on the main thread to modify the DataGridView's DataSource and customize it.
		/// Logs the process and handles errors gracefully.
		/// </summary>
		/// <param name="users">The list of users to display in the DataGridView.</param>
		/// <remarks>
		/// This method ensures that the DataGridView is updated on the UI thread using the <c>Invoke</c> method.
		/// It sets the <c>DataSource</c> of the DataGridView to the provided list of users and then customizes the appearance of the grid.
		/// </remarks>
		private void UpdateDataGridViewAsync(List<dynamic> users)
		{
			try
			{
				// Log the beginning of the update process
				_logger.LogInformation("Starting to update DataGridView with user data.");

				// Invoke the update on the UI thread to modify the DataGridView safely
				this.Invoke(new Action(() =>
				{
					// Check if the users list is null or empty
					if (users == null || users.Count == 0)
					{
						_logger.LogWarning("No users to display. The list is empty or null.");
						return;
					}

					// Log the number of users being displayed
					_logger.LogInformation($"Displaying {users.Count} users in the DataGridView.");

					// Set the new data source for the grid
					dataGridViewForMembers.DataSource = users;

					// Customize the grid appearance (column width, formatting, etc.)
					DataGridViewController.CustomizeDataGridView(dataGridViewForMembers);
				}));

				// Log the completion of the update process
				_logger.LogInformation("DataGridView update completed successfully.");
			}
			catch (Exception ex)
			{
				// Log the exception details
				_logger.LogError(ex, "An error occurred while updating the DataGridView.");

				// Optionally, inform the user of the error
				MessageBox.Show("An error occurred while updating the data. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		/// <summary>
		/// Asynchronously resets the progress bar.
		/// This method ensures that the progress bar is reset safely on the UI thread and logs the process.
		/// </summary>
		private async Task ResetProgressBarAsync()
		{
			try
			{
				// Log the beginning of the progress bar reset
				_logger.LogInformation("Starting to reset the progress bar.");

				// Check if the progress bar is disposed
				if (pbMembers.IsDisposed)
				{
					_logger.LogWarning("Progress bar is disposed, skipping reset.");
					return;
				}

				// Reset the progress bar asynchronously
				await Task.Run(() =>
				{
					this.BeginInvoke(async () =>
					{
						// Call the method to reset the progress bar
						await ProgressBarController.pbProgressReset(pbMembers);
					});
				});

				// Log the successful reset of the progress bar
				_logger.LogInformation("Progress bar has been reset successfully.");
			}
			catch (Exception ex)
			{
				// Log any exceptions that occur during the reset process
				_logger.LogError(ex, "An error occurred while resetting the progress bar.");

				// Optionally, inform the user of the error
				MessageBox.Show("An error occurred while resetting the progress bar. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		/// <summary>
		/// Handles the click event for the "Exit" menu item. Closes the form.
		/// Logs the exit action and handles any exceptions that might occur during the process.
		/// </summary>
		private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				// Log the user's action to exit the application
				_logger.LogInformation("User triggered the Exit menu item.");

				// Close the current form
				this.Close();

				// Log the successful closing of the form
				_logger.LogInformation("Form closed successfully.");
			}
			catch (Exception ex)
			{
				// Log any exceptions that occur during the form close operation
				_logger.LogError(ex, "An error occurred while trying to close the form.");

				// Optionally, inform the user of the error
				MessageBox.Show("An error occurred while closing the application. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		/// <summary>
		/// Handles the form closing event for the "FMembers" form.
		/// Cancels ongoing tasks and logs the closing process.
		/// </summary>
		private void FMembers_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				// Log the attempt to close the form
				_logger.LogInformation("FMembers form is closing.");

				// Cancel any ongoing tasks (such as data loading or long-running operations)
				CancellationTokenSource?.Cancel();

				// Log the cancellation of tasks (if applicable)
				_logger.LogInformation("CancellationTokenSource has been canceled to stop ongoing tasks.");

				if (e.CloseReason == CloseReason.UserClosing)
				{
					var result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo);
					if (result == DialogResult.No)
					{
						e.Cancel = true; // Prevent the form from closing
						_logger.LogInformation("User canceled the form closing.");
					}
				}

				// Log successful form closing
				_logger.LogInformation("FMembers form closed successfully.");
			}
			catch (Exception ex)
			{
				// Log any errors that occur during the form closing process
				_logger.LogError(ex, "An error occurred during the form closing process.");
			}
		}

		/// <summary>
		/// Clears IIN textbox when clicked on it.
		/// Logs the click event and handles any potential errors.
		/// </summary>
		private void TbIINSearch_Click(object sender, EventArgs e)
		{
			try
			{
				// Log the click event for the IIN search textbox
				_logger.LogInformation("IIN Search TextBox clicked, clearing the text.");

				// Clear the textbox content
				TbIINSearch.Text = "";
			}
			catch (Exception ex)
			{
				// Log any errors that occur when clearing the textbox
				_logger.LogError(ex, "An error occurred while clearing the IIN Search TextBox.");
			}
		}

		/// <summary>
		/// Handles the click event of the "Le" menu item, which processes the borrowing action for a selected member.
		/// If the selected member has a valid IIN, it invokes an event to initiate the borrowing process and shows the borrowing form.
		/// Logs the progress and errors during the process.
		/// </summary>
		private async void LeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				// Try to get IIN from the selected row in the DataGridView
				(bool isValid, long IIN) = DataGridViewController.TryGetIINFromRow(dataGridViewForMembers);

				if (isValid)
				{
					// Log that a valid member was selected for borrowing
					_logger.LogInformation("Valid member selected with IIN: {IIN}. Proceeding with borrowing.", IIN);

					// If a valid member is selected, invoke the borrowing event
					MemberCreateOrUpdateEvent?.Invoke(new MemberEventArgs("BORROW", IIN));

					// Show the borrowing form as a dialog
					_logger.LogInformation("Showing borrowing form dialog.");
					FlendOrRecieveBook.ShowDialog();

					// Log the data refresh after borrowing
					_logger.LogInformation("Refreshing data grid after borrowing process.");
					await RefreshDataGridForMembers();
				}
				else
				{
					// Log when no valid member is selected
					_logger.LogWarning("No valid member selected for borrowing.");

					// Show an error message if no member is selected
					MessageBox.Show("Please select a member to proceed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
			catch (Exception ex)
			{
				// Log any errors that occur during the process
				_logger.LogError(ex, "An error occurred while processing the borrowing action.");
			}
		}


		/// <summary>
		/// Handles the click event of the "See Lended Books for This Member" menu item.
		/// Checks if a member has borrowed any books and, if so, displays their information in another form.
		/// Logs actions and handles errors during the process.
		/// </summary>
		private async void SeeLendedBooksForThisMemberToolStripMenuItem_Click(object sender, EventArgs e) //TODO use Repository
		{
			// Check if a valid member is selected and retrieve their IIN
			(bool isValid, long IIN) = DataGridViewController.TryGetIINFromRow(dataGridViewForMembers);

			if (isValid)
			{
				try
				{
					_logger.LogInformation("Valid member selected with IIN: {IIN}. Fetching borrowed books information.", IIN);

					// Using the repository instead of directly querying the database
					var memberData = await GetMemberWithBorrowedBooksAsync(IIN);

					if (memberData == null)
					{
						_logger.LogWarning("No member found with IIN: {IIN}.", IIN);
						MessageBox.Show($"Member with IIN: {IIN} not found.");
						return;
					}

					// Log if the member has borrowed books or not
					if (memberData.Books.Any())
					{
						_logger.LogInformation("Member has borrowed books. Triggering the return event for IIN: {IIN}.", IIN);

						// If the member has borrowed books, trigger the event and show the form
						MemberCreateOrUpdateEvent?.Invoke(new MemberEventArgs("RETURN", IIN));
						FlendOrRecieveBook.ShowDialog();

						// Refresh the data grid after the operation
						_logger.LogInformation("Refreshing data grid after the return operation for IIN: {IIN}.", IIN);
						await RefreshDataGridForMembers();
					}
					else
					{
						_logger.LogInformation("Member has not borrowed any books. Showing message to the user.");
						MessageBox.Show($"{memberData.Name} {memberData.Surname} has not borrowed any books yet.");
					}
				}
				catch (Exception ex)
				{
					// Log and show an error message if any exception occurs
					_logger.LogError(ex, "An error occurred while fetching lended books for member with IIN: {IIN}.", IIN);
					MessageBox.Show($"An error occurred: {ex.Message}");
				}
			}
			else
			{
				_logger.LogWarning("No valid member selected. Cannot proceed with the operation.");
				MessageBox.Show("Please select a valid member to proceed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
		public async Task<dynamic> GetMemberWithBorrowedBooksAsync(long IIN)
		{
			try
			{
				// Log entry into the method
				_logger.LogInformation("Attempting to retrieve member data and borrowed books for IIN: {IIN}", IIN);

				// Define the projection to retrieve member data along with their borrowed books
				var result = await _memberRepository.GetWithProjectionAsync(
					m => new
					{
						m.IIN,
						m.Name,
						m.Surname,
						Books = m.Books.Select(b => new
						{
							b.Id,
							b.Title,
							b.Genre
						}).ToList()
					},
					m => m.Books // Include the Books navigation property
				);

				// Log the success of data retrieval
				_logger.LogInformation("Successfully retrieved member data and borrowed books for IIN: {IIN}", IIN);

				// Filter the result to match the specific IIN
				var member = result.FirstOrDefault(m => m.IIN == IIN);

				// Log the result of the filtering
				if (member != null)
				{
					_logger.LogInformation("Member found for IIN: {IIN}", IIN);
				}
				else
				{
					_logger.LogWarning("No member found for IIN: {IIN}", IIN);
				}

				return member;
			}
			catch (Exception ex)
			{
				// Log the exception with relevant details
				_logger.LogError(ex, "An error occurred while fetching member data and borrowed books for IIN: {IIN}", IIN);
				throw; // Re-throw the exception after logging it
			}
		}
	}
}
