using Library.Domain.Models;
using Library.Presentation.Controllers;
using Library.Presentation.Controllers.PictureController;
using Microsoft.Extensions.Logging;
using Library.Application.Services.CRUD;
using Library.Infrastructure.Repositories;
namespace Library
{
	public partial class FormMembers : Form
	{//TODO Handle zero members in db via service 
		#region FieldsAndProperties
		readonly GenericRepository<Member> _memberRepository;
		readonly ILogger _logger;
		internal FormBorrowOrReturnBook FlendOrRecieveBook { get; private set; } //TODO:CRITICAL logic of borrow and return book is broken
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
			FlendOrRecieveBook = new FormBorrowOrReturnBook();
			FaddEdit_prop = new FaddEdit_prop();
			_logger = LoggerService.CreateLogger<FormMembers>();
			_memberRepository = new();
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
			(bool isValid, long IIN) = LibraryService.TryGetValueFromRow<long>(dataGridViewForMembers);

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
			(bool IsValid, long IIN) = LibraryService.TryGetValueFromRow<long>(dataGridViewForMembers);
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
					if (await _memberRepository.DeleteAsync(new LibraryContextForEFcore(), m => m.IIN, IIN) == true)
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
		private int previousTextLength = 0; // Track the previous length of the input text

		/// <summary>
		/// Handles the text changed event of the TbIINSearch text box. Searches for members
		/// based on the input IIN. If the input is a valid IIN, the members matching the IIN
		/// will be displayed. Otherwise, all members will be shown. In case of an error, 
		/// it logs the exception and shows a message to the user.
		/// </summary>
		/// <param name="sender">The object that triggered the event.</param>
		/// <param name="e">The event arguments.</param>
		private async void TbIINSearch_TextChanged(object sender, EventArgs e)
		{
			_logger.LogInformation("Entered TbIINSearch_TextChanged method. Text length: {TextLength}", TbIINSearch.Text.Length);
			var memberService = new MemberService(_logger, _memberRepository);

			// If the current text length is less than or equal to 3 and it was greater than 3 previously, perform the fetch
			if (TbIINSearch.Text.Length <= 3 && previousTextLength > 3)
			{
				dataGridViewForMembers.DataSource = await memberService.GetMembersAsync();
				_logger.LogInformation("All members loaded and displayed in DataGridView.");
			}
			else if (TbIINSearch.Text.Length > 3)
			{
				// When text length is greater than 3, perform the filtered search
				string input = TbIINSearch.Text.Trim();
				_logger.LogInformation("User entered input: {Input}", input);

				if (long.TryParse(input, out long IIN) && IIN != 0)
				{
					_logger.LogInformation("Valid IIN parsed: {IIN}", IIN);
					dataGridViewForMembers.DataSource = await memberService.GetFilteredMembersAsync(IIN);
					_logger.LogInformation("Matched members found and displayed in DataGridView.");
				}
				else
				{
					dataGridViewForMembers.DataSource = null;
					_logger.LogWarning("Invalid IIN input: {Input}", input);
				}
			}

			// Update previous text length after processing
			previousTextLength = TbIINSearch.Text.Length;
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

						var memberService = new MemberService(_logger, _memberRepository);
						// Get the total count of members
						int totalMembersCount = await GetTotalMembersCountAsync();
						_logger.LogInformation("Total members count retrieved: {TotalCount}", totalMembersCount);

						// Check if the operation was cancelled
						if (CancellationToken.IsCancellationRequested) return;

						// Update the progress bar as data is being loaded
						await UpdateProgressBarAsync(0, 25);
						_logger.LogInformation("Progress bar updated to 25%.");

						// Retrieve members from the database
						var users = await memberService.GetMembersAsync();
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
				int totalMembersCount = await Task.Run(() => new LibraryContextForEFcore().Members.Count());

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
				await ProgressBarController.UpdateProgressAsync(this, pbMembers, minValue, maxValue);

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
					this.CreateControl(); //Handle if form exit sonner then were initialized
					this.BeginInvoke(async () =>
					{
						// Call the method to reset the progress bar
						await ProgressBarController.ResetProgressAsync(pbMembers);
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
		private async void BorrowBookToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				// Try to get IIN from the selected row in the DataGridView
				(bool isValid, long IIN) = LibraryService.TryGetValueFromRow<long>(dataGridViewForMembers);

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
		private async void ReturnBookToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Check if a valid member is selected and retrieve their IIN
			(bool isValid, long IIN) = LibraryService.TryGetValueFromRow<long>(dataGridViewForMembers);
			if (!isValid)
			{
				_logger.LogWarning("No valid member selected. Cannot proceed with the operation.");
				MessageBox.Show("Please select a valid member to proceed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			_logger.LogInformation("Valid member selected with IIN: {IIN}. Fetching borrowed books information.", IIN);

			try
			{
				var memberService = new MemberService(_logger, _memberRepository);

				// Using the repository instead of directly querying the database
				var memberWithBooks = await memberService.GetMemberWithBorrowedBooksAsync(IIN);
				if (memberWithBooks == null)
				{
					_logger.LogWarning("No member found with IIN: {IIN}.", IIN);
					MessageBox.Show($"Member with IIN: {IIN} not found.");
					return;
				}

				// Guard clause: If the member has not borrowed any books
				if (memberWithBooks.Books.Count <= 0)
				{
					_logger.LogInformation("Member has not borrowed any books. Showing message to the user.");
					MessageBox.Show($"{memberWithBooks.Name} {memberWithBooks.Surname} has not borrowed any books yet.");
					return;
				}

				// Member has borrowed books, proceed with the return operation
				_logger.LogInformation("Member has borrowed books. Triggering the return event for IIN: {IIN}.", IIN);
				MemberCreateOrUpdateEvent?.Invoke(new MemberEventArgs("RETURN", IIN));
				FlendOrRecieveBook.ShowDialog();

				// Refresh the data grid after the operation
				_logger.LogInformation("Refreshing data grid after the return operation for IIN: {IIN}.", IIN);
				await RefreshDataGridForMembers();
			}
			catch (Exception ex)
			{
				// Handle any exceptions
				_logger.LogError(ex, "An error occurred while fetching lended books for member with IIN: {IIN}.", IIN);
				MessageBox.Show($"An error occurred: {ex.Message}");
			}
		}


		/// <summary>
		/// Handles the double-click event on a cell in the DataGridView for members.
		/// </summary>
		/// <param name="sender">The source of the event, typically the DataGridView.</param>
		/// <param name="e">Provides data for the <see cref="DataGridViewCellMouseEventArgs"/> event.</param>
		/// <remarks>
		/// This method:
		/// - Checks if the left mouse button was double-clicked on a valid cell (not a header).
		/// - Retrieves the IIN (Individual Identification Number) from the clicked row.
		/// - Invokes the <see cref="MemberCreateOrUpdateEvent"/> for editing the member.
		/// - Displays a dialog for editing the member's details.
		/// </remarks>
		/// <exception cref="Exception">
		/// Logs any exceptions that occur while displaying the edit dialog.
		/// </exception>
		private void DataGridViewForMembers_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			_logger.LogInformation("Entered dataGridViewForMembers_CellMouseDoubleClick method");

			// Guard clause: Ensure the left mouse button was double-clicked on a valid cell
			if (e.Button != MouseButtons.Left || e.RowIndex < 0 || e.ColumnIndex < 0)
			{
				_logger.LogWarning("Mouse double-click occurred outside a valid cell (row: {RowIndex}, column: {ColumnIndex})", e.RowIndex, e.ColumnIndex);
				return;
			}

			_logger.LogInformation("Left mouse button double-clicked on valid cell at row {RowIndex}, column {ColumnIndex}", e.RowIndex, e.ColumnIndex);

			// Set the current cell to the clicked one
			dataGridViewForMembers.CurrentCell = dataGridViewForMembers.Rows[e.RowIndex].Cells[e.ColumnIndex];

			// Attempt to retrieve the IIN from the selected row
			var (isValid, IIN) = LibraryService.TryGetValueFromRow<long>(dataGridViewForMembers);
			if (!isValid)
			{
				_logger.LogWarning("Failed to retrieve a valid IIN from row {RowIndex}", e.RowIndex);
				return;
			}

			_logger.LogInformation("Valid IIN ({IIN}) retrieved from row {RowIndex}", IIN, e.RowIndex);

			// Guard clause: Check if the event has subscribers
			if (MemberCreateOrUpdateEvent == null)
			{
				_logger.LogWarning("MemberCreateOrUpdateEvent has no subscribers");
				return;
			}

			// Invoke the edit event with the IIN
			MemberCreateOrUpdateEvent.Invoke(new MemberEventArgs("EDIT", IIN));
			_logger.LogInformation("MemberCreateOrUpdateEvent invoked for IIN: {IIN}", IIN);

			try
			{
				// Show the edit form dialog
				FaddEdit_prop.ShowDialog();
				_logger.LogInformation("FaddEdit_prop dialog closed successfully");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while displaying FaddEdit_prop dialog");
			}
		}


		/// <summary>
		/// Handles the event when the current cell in the DataGridView for members changes.
		/// </summary>
		/// <param name="sender">The source of the event, typically the DataGridView.</param>
		/// <param name="e">Provides data for the event.</param>
		/// <remarks>
		/// This method:
		/// - Validates if the selected cell is not null.
		/// - Attempts to retrieve the IIN (Individual Identification Number) from the selected row.
		/// - Uses the IIN to find the corresponding member from the repository.
		/// - Displays the member's photo in a PictureBox.
		/// </remarks>
		/// <exception cref="Exception">
		/// Catches and logs any exceptions that occur while fetching the member's data or photo.
		/// </exception>
		private async void DataGridViewForMembers_CurrentCellChanged(object sender, EventArgs e)
		{
			// Ensure the current cell is not null
			if (dataGridViewForMembers.CurrentCell == null) return;

			try
			{
				// Attempt to retrieve the IIN from the current row
				var (isValid, IIN) = LibraryService.TryGetValueFromRow<long>(dataGridViewForMembers);

				if (isValid)
				{
					// Fetch the member data asynchronously
					var findedMember = await _memberRepository.GetByFieldAsync(new LibraryContextForEFcore(), m => m.IIN, IIN);
					if (findedMember != null)
					{
						// Convert the member's photo byte array to an Image and display it
						pictureBoxMember.Image = PictureController.ConvertByteToImage(findedMember.Photo);
					}
					else
					{
						MessageBox.Show($"Member with IIN {IIN} was not found.", "Member Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}
				else
				{
					MessageBox.Show("Invalid or missing IIN in the selected row.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
			catch (Exception ex)
			{
				// Log the exception and show an error message to the user
				_logger.LogError(ex, "An error occurred while fetching the member's data or displaying the photo.");
				MessageBox.Show($"An error occurred while retrieving the member's data. Error message: {ex.Message}. Please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
