using Library.Data;
using Library.Services;
using Library.Models;
using Library.Data.Repositories;
using Library.Presentation.Controllers;
using Library.Presentation.Controllers.PictureController;
using Library.Presentation.Theme;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
namespace Library
{
	//TODO datagrid not updating after add/edit member
	public partial class FormMembers : Form //TODO context per form, not per method
	{//TODO Handle zero members in db via service 
		#region FieldsAndProperties
		readonly Repository<Member> _memberRepository; //TODO factory for services,repositories? and forms
		readonly ILogger _logger;
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal CancellationTokenSource CancellationTokenSource { get; set; }
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal CancellationToken CancellationToken { get; set; }
		readonly ControlsHelper controlsController = new();

		readonly LibraryContext _context = new(); //Disposed in desighner file

		public FormMembers()
		{
			InitializeComponent();
			buttonLoadPhoto.Tag = ButtonRole.Secondary;
			AppTheme.Apply(this);
			CancellationTokenSource = new CancellationTokenSource();
			_logger = LoggerService.CreateLogger<FormMembers>();
			_memberRepository = new();
		}
		#endregion

		private async void AddMemberToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_logger.LogInformation("AddMemberToolStripMenuItem_Click: Started processing the event.");
			try
			{
				this.Cursor = Cursors.WaitCursor;

				using FaddEdit_prop _FaddEdit_prop = new();
				_FaddEdit_prop.ActionMode = "CREATE";
				this.Cursor = Cursors.Default;

				_FaddEdit_prop.ShowDialog();

				await RefreshDataGridForMembers();
			}
			catch (Exception ex)
			{
				this.Cursor = Cursors.Default;
				_logger.LogError(ex, "An error occurred while handling AddMemberToolStripMenuItem_Click");
				MessageBoxHelper.ShowError($"An unexpected error occurred: {ex.Message}\nPlease contact support if the problem persists.");
			}
		}

		private async void FMembers_Load(object sender, EventArgs e)
		{
			// Embedded into the main window: closing is handled there
			exitToolStripMenuItem.Visible = TopLevel;

			await RefreshDataGridForMembers();
		}

		private async void EditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_logger.LogInformation("EditToolStripMenuItem_Click: Started processing the event.");
			(bool isValid, long IIN) = DataGridViewHelper.TryGetValueFromRow<long>(dataGridViewForMembers);

			if (isValid)
			{
				using FaddEdit_prop _FaddEdit_prop = new();
				_FaddEdit_prop.ActionMode = "UPDATE";
				_FaddEdit_prop.MemberIIN = IIN;
				_FaddEdit_prop.ShowDialog();
				await RefreshDataGridForMembers();
			}
		}

		private async void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_logger.LogInformation("DeleteToolStripMenuItem_Click: Started processing the event.");

			(bool IsValid, long IIN) = DataGridViewHelper.TryGetValueFromRow<long>(dataGridViewForMembers);

			if (IsValid)
			{
				bool ifConfirmed = MessageBoxHelper.ShowConfirmation("Are you sure to remove?", "Removing member");

				if (ifConfirmed)
				{
					if (await _memberRepository.DeleteAsync(new LibraryContext(), m => m.IIN, IIN) == true)
					{
						MessageBoxHelper.ShowSuccess("Member successfully removed");

						await RefreshDataGridForMembers();
					}
				}
			}
			else
			{
				MessageBoxHelper.ShowWarning("Cannot delete this member, try later or contact your system administrator");
			}
		}

		private void View_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
			{
				dataGridViewForMembers.CurrentCell = dataGridViewForMembers.Rows[e.RowIndex].Cells[e.ColumnIndex];

				Point relativeCursorPosition = dataGridViewForMembers.PointToClient(Cursor.Position);
				cmMember.Show(dataGridViewForMembers, relativeCursorPosition);
			}
		}
		private int previousTextLength = 0;

		private async void TbIINSearch_TextChanged(object sender, EventArgs e)
		{
			_logger.LogInformation("Entered TbIINSearch_TextChanged method. Text length: {TextLength}", TbIINSearch.Text.Length);
			var memberService = new MemberService(_logger, _memberRepository, new LibraryContext());

			if (TbIINSearch.Text.Length <= 3 && previousTextLength > 3)
			{
				dataGridViewForMembers.DataSource = await memberService.GetMembersAsync(_context);
			}
			else if (TbIINSearch.Text.Length > 3)
			{
				string input = TbIINSearch.Text.Trim();

				if (long.TryParse(input, out long IIN) && IIN != 0)
				{
					dataGridViewForMembers.DataSource = await memberService.GetFilteredMembersAsync(_context, IIN);
				}
				else
				{
					dataGridViewForMembers.DataSource = null;
				}
			}

			previousTextLength = TbIINSearch.Text.Length;
		}

		private async Task RefreshDataGridForMembers()
		{
			_logger.LogInformation("Entered RefreshDataGridForMembers method.");

			try
			{
				await SetControlsEnabledAsync(false);

				CancellationToken = CancellationTokenSource.Token;

				await Task.Run(async () =>
				{
					try
					{
						var memberService = new MemberService(_logger, _memberRepository, new LibraryContext());
						int totalMembersCount = await GetTotalMembersCountAsync();

						if (CancellationToken.IsCancellationRequested) return;

						await UpdateProgressBarAsync(0, 25);

						var users = await memberService.GetMembersAsync(_context);

						if (CancellationToken.IsCancellationRequested) return;

						await UpdateProgressBarAsync(25, 50);

						UpdateDataGridViewAsync(users);

						if (CancellationToken.IsCancellationRequested) return;

						await UpdateProgressBarAsync(50, 100);

						if (CancellationToken.IsCancellationRequested) return;

						if (pbMembers.IsDisposed) return;

						await ResetProgressBarAsync();
						await SetControlsEnabledAsync(true);
					}
					catch (Exception ex)
					{
						ErrorHandler.HandleException(ex, "An error occurred while loading members");
					}
				}, CancellationToken);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred in RefreshDataGridForMembers method.");
				MessageBoxHelper.ShowError($"An error with message:{ex.Message}, occurred while preparing data for display. Please try again later.");
			}
		}

		private async Task SetControlsEnabledAsync(bool isEnabled)
		{
			await controlsController.SetControlsEnableFlag(this, this.Controls, isEnabled);
		}

		private async Task<int> GetTotalMembersCountAsync()
		{
			try
			{
				int totalMembersCount = await Task.Run(() => new LibraryContext().Members.Count());
				return totalMembersCount;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while retrieving the total members count.");
				MessageBoxHelper.ShowError("An error occurred while retrieving the total members count. Please try again.");
				return 0;
			}
		}

		private async Task UpdateProgressBarAsync(int minValue, int maxValue)
		{
			try
			{
				await ProgressBarHelper.UpdateProgressAsync(this, pbMembers, minValue, maxValue);
			}
			catch (ArgumentOutOfRangeException ex)
			{
				_logger.LogError(ex, "Invalid progress bar range: minValue = {MinValue}, maxValue = {MaxValue}.", minValue, maxValue);
				MessageBoxHelper.ShowError("Error: Invalid range for the progress bar.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while updating the progress bar.");
				MessageBoxHelper.ShowError("An unexpected error occurred while updating the progress bar.");
			}
		}

		private void UpdateDataGridViewAsync(List<dynamic> users)
		{
			try
			{
				this.Invoke(new Action(() =>
				{
					if (users == null || users.Count == 0)
					{
						return;
					}

					dataGridViewForMembers.DataSource = users;
					DataGridViewHelper.CustomizeDataGridView(dataGridViewForMembers);
				}));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while updating the DataGridView.");
				MessageBoxHelper.ShowError("An error occurred while updating the data. Please try again later.");
			}
		}

		private async Task ResetProgressBarAsync()
		{
			try
			{
				if (pbMembers.IsDisposed)
				{
					return;
				}

				await Task.Run(() =>
				{
					this.CreateControl();
					this.BeginInvoke(async () =>
					{
						await ProgressBarHelper.ResetProgressAsync(pbMembers);
					});
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while resetting the progress bar.");
				MessageBoxHelper.ShowError("An error occurred while resetting the progress bar. Please try again later.");
			}
		}

		private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while trying to close the form.");
				MessageBoxHelper.ShowError("An error occurred while closing the application. Please try again later.");
			}
		}

		private void FMembers_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (e.CloseReason == CloseReason.UserClosing)
				{
					var result = MessageBoxHelper.ShowConfirmation("Are you sure you want to exit?", "Confirm Exit");
					if (!result)
					{
						e.Cancel = true;
						return;
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred during the form closing process.");
			}
		}

		private async void BorrowBookToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				(bool isValid, long IIN) = DataGridViewHelper.TryGetValueFromRow<long>(dataGridViewForMembers);

				if (isValid)
				{
					using FormBorrowOrReturnBook FlendOrRecieveBook  = new ();
					FlendOrRecieveBook.ActionMode = "BORROW";
					FlendOrRecieveBook.MemberIIN = IIN;
					FlendOrRecieveBook.ShowDialog();
					await RefreshDataGridForMembers();
				}
				else
				{
					MessageBoxHelper.ShowWarning("Please select a member to proceed.");
				}
			}
			catch (Exception ex)
			{
				ErrorHandler.HandleException(ex, "An error occurred while processing the borrowing action.");
			}
		}

		private async void ReturnBookToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(bool isValid, long IIN) = DataGridViewHelper.TryGetValueFromRow<long>(dataGridViewForMembers);
			if (!isValid)
			{
				MessageBoxHelper.ShowWarning("Please select a valid member to proceed.");
				return;
			}

			try
			{
				var memberService = new MemberService(_logger, _memberRepository, new LibraryContext());

				int borrowedBooksCount = await memberService.GetBorrowedBooksCountAsync(_context, IIN);
				if (borrowedBooksCount == 0)
				{
					MessageBoxHelper.ShowWarning($"Member with IIN: {IIN} has not borrowed any books yet.");
					return;
				}
				using FormBorrowOrReturnBook FlendOrRecieveBook = new();
				FlendOrRecieveBook.ActionMode = "RETURN";
				FlendOrRecieveBook.MemberIIN = IIN;
				FlendOrRecieveBook.ShowDialog();

				await RefreshDataGridForMembers();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while checking borrowed books for member with IIN: {IIN}.", IIN);
				MessageBoxHelper.ShowError($"An error occurred: {ex.Message}");
			}
		}

		private void DataGridViewForMembers_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left || e.RowIndex < 0 || e.ColumnIndex < 0)
			{
				return;
			}

			dataGridViewForMembers.CurrentCell = dataGridViewForMembers.Rows[e.RowIndex].Cells[e.ColumnIndex];

			var (isValid, IIN) = DataGridViewHelper.TryGetValueFromRow<long>(dataGridViewForMembers);
			if (!isValid)
			{
				return;
			}

			try
			{
				using FaddEdit_prop _FaddEdit_prop = new();
				_FaddEdit_prop.ActionMode = "UPDATE";
				_FaddEdit_prop.MemberIIN = IIN;
				_FaddEdit_prop.ShowDialog();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while displaying FaddEdit_prop dialog");
			}
		}

		private async void buttonLoadPhoto_Click(object sender, EventArgs e)
		{
			if (dataGridViewForMembers.CurrentCell == null) return;

			try
			{
				var (isValid, IIN) = DataGridViewHelper.TryGetValueFromRow<long>(dataGridViewForMembers);

				if (isValid)
				{
					var memberService = new MemberService(_logger, _memberRepository, new LibraryContext());
					var findedPhoto = await memberService.GetMemberPhotoByIINAsync(_context, IIN);
					if (findedPhoto != null)
					{
						pictureBoxMember.Image = PictureHelper.ConvertByteToImage(findedPhoto);
					}
					else
					{
						MessageBoxHelper.ShowWarning($"Member with IIN {IIN} was not found.");
					}
				}
				else
				{
					MessageBoxHelper.ShowWarning("Invalid or missing IIN in the selected row");
				}
			}
			catch (Exception ex)
			{
				ErrorHandler.HandleException(ex, "An error occurred while fetching the member's data or displaying the photo.");
			}
		}
	}
}