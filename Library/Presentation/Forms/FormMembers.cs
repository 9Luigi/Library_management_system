using Library.Data;
using Library.Services;
using Library.Models;
using Library.Data.Repositories;
using Library.Presentation.Controllers;
using Library.Presentation.Controllers.PictureController;
using Library.Presentation.Settings;
using Library.Presentation.Theme;
using Microsoft.Extensions.Logging;
namespace Library
{
	//TODO datagrid not updating after add/edit member
	public partial class FormMembers : Form //TODO context per form, not per method
	{//TODO Handle zero members in db via service 
		#region FieldsAndProperties
		readonly Repository<Member> _memberRepository; //TODO factory for services,repositories? and forms
		readonly ILogger _logger;

		readonly LibraryContext _context = new(); //Disposed in desighner file

		public FormMembers()
		{
			InitializeComponent();
			bPrevPage.Tag = ButtonRole.Secondary;
			bNextPage.Tag = ButtonRole.Secondary;
			bClearSearch.Tag = ButtonRole.Secondary;
			AppTheme.Apply(this);
			_logger = LoggerService.CreateLogger<FormMembers>();
			_memberRepository = new();
			InitializePager();
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
					using var deleteContext = new LibraryContext();
					var memberService = new MemberService(_logger, _memberRepository, deleteContext);
					if (await memberService.DeleteMemberAsync(deleteContext, IIN))
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
		#region Search

		private const int IinLength = 12;
		private long _searchIin;

		private async void BSearch_Click(object sender, EventArgs e) => await SearchAsync();

		private async void TbIINSearch_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Enter) return;

			e.SuppressKeyPress = true; // no system "ding"
			await SearchAsync();
		}

		private void TbIINSearch_KeyPress(object sender, KeyPressEventArgs e)
		{
			// Digits and control keys (Backspace, Ctrl+V, ...) only
			if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
		}

		private async void TbIINSearch_TextChanged(object sender, EventArgs e)
		{
			// Field emptied (by hand or by Clear): back to the list
			if (TbIINSearch.Text.Length == 0 && _isSearchMode) await ExitSearchAsync();
		}

		private void BClearSearch_Click(object sender, EventArgs e)
		{
			TbIINSearch.Clear(); // TextChanged returns to the list
			TbIINSearch.Focus();
		}

		private async Task SearchAsync()
		{
			string input = TbIINSearch.Text.Trim();
			if (input.Length == 0)
			{
				if (_isSearchMode) await ExitSearchAsync();
				return;
			}

			// The field blocks letters on typing, but pasted text still has to be checked
			if (input.Length != IinLength || !input.All(char.IsDigit))
			{
				MessageBoxHelper.ShowWarning($"IIN must be {IinLength} digits.");
				return;
			}

			_searchIin = long.Parse(input);
			_isSearchMode = true;
			await ShowSearchResultsAsync();
		}

		private async Task ExitSearchAsync()
		{
			// Back to the page that was shown before the search
			_isSearchMode = false;
			await RefreshDataGridForMembers();
		}

		#endregion

		private async Task ShowSearchResultsAsync()
		{
			try
			{
				using var searchContext = new LibraryContext();
				var memberService = new MemberService(_logger, _memberRepository, searchContext);
				var found = await memberService.GetFilteredMembersAsync(searchContext, _searchIin);

				dataGridViewForMembers.DataSource = found;
				DataGridViewHelper.CustomizeDataGridView(dataGridViewForMembers);
				lblRange.Text = $"Found: {found.Count}";
			}
			catch (Exception ex)
			{
				ErrorHandler.HandleException(ex, "An error occurred while searching members");
			}
			finally
			{
				UpdatePagerState();
			}
		}

		/// <summary>
		/// Reloads what the grid shows: search results while searching, otherwise the current page.
		/// </summary>
		private async Task RefreshDataGridForMembers()
		{
			_logger.LogInformation("Entered RefreshDataGridForMembers method.");

			if (_isSearchMode)
			{
				await ShowSearchResultsAsync();
				return;
			}

			try
			{
				// Only the pager is locked (against double clicks); the load itself takes milliseconds
				bPrevPage.Enabled = bNextPage.Enabled = cbPageSize.Enabled = false;

				using var context = new LibraryContext();
				var memberService = new MemberService(_logger, _memberRepository, context);
				var (members, totalCount) = await memberService.GetMembersPageAsync(context, _currentPage, PageSize);

				_totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)PageSize));
				if (_currentPage > _totalPages)
				{
					// e.g. the only member on the last page was deleted
					_currentPage = _totalPages;
					(members, totalCount) = await memberService.GetMembersPageAsync(context, _currentPage, PageSize);
				}

				dataGridViewForMembers.DataSource = members;
				DataGridViewHelper.CustomizeDataGridView(dataGridViewForMembers);

				int first = totalCount == 0 ? 0 : (_currentPage - 1) * PageSize + 1;
				int last = (_currentPage - 1) * PageSize + members.Count;
				lblRange.Text = $"{first}–{last} of {totalCount} members";
			}
			catch (Exception ex)
			{
				ErrorHandler.HandleException(ex, "An error occurred while loading members");
			}
			finally
			{
				// Runs after errors too, so the pager never stays locked
				UpdatePagerState();
			}
		}

		#region Paging

		private int _currentPage = 1;
		private int _totalPages = 1;
		private bool _isSearchMode;
		private bool _pagerInitializing;

		private static int PageSize => UserSettings.Current.MembersPageSize;

		private void InitializePager()
		{
			_pagerInitializing = true;
			cbPageSize.Items.AddRange(UserSettings.PageSizes.Cast<object>().ToArray());
			cbPageSize.SelectedItem = PageSize;
			_pagerInitializing = false;
		}

		private async void CbPageSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_pagerInitializing || cbPageSize.SelectedItem is not int pageSize) return;

			UserSettings.Current.MembersPageSize = pageSize;
			UserSettings.Current.Save();
			_currentPage = 1;
			await RefreshDataGridForMembers();
		}

		private async void BPrevPage_Click(object sender, EventArgs e)
		{
			if (_currentPage <= 1) return;
			_currentPage--;
			await RefreshDataGridForMembers();
		}

		private async void BNextPage_Click(object sender, EventArgs e)
		{
			if (_currentPage >= _totalPages) return;
			_currentPage++;
			await RefreshDataGridForMembers();
		}

		/// <summary>
		/// Paging is switched off while search results are shown.
		/// </summary>
		private void UpdatePagerState()
		{
			bPrevPage.Enabled = !_isSearchMode && _currentPage > 1;
			bNextPage.Enabled = !_isSearchMode && _currentPage < _totalPages;
			cbPageSize.Enabled = !_isSearchMode;
			lblPageInfo.Text = _isSearchMode ? "" : $"Page {_currentPage} of {_totalPages}";
		}

		#endregion

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

		private int _photoRequestId;

		/// <summary>
		/// Shows the photo of the selected member as soon as the selection changes.
		/// </summary>
		private async void DataGridViewForMembers_SelectionChanged(object sender, EventArgs e)
		{
			var (isValid, IIN) = DataGridViewHelper.TryGetValueFromRow<long>(dataGridViewForMembers);
			if (!isValid)
			{
				SetMemberPhoto(null);
				return;
			}

			// The selection can change faster than photos load: only the latest request may update the picture
			int requestId = ++_photoRequestId;
			try
			{
				// Own context: the form's _context may be busy with a grid refresh at the same moment
				using var photoContext = new LibraryContext();
				var memberService = new MemberService(_logger, _memberRepository, photoContext);
				var photo = await memberService.GetMemberPhotoAsync(photoContext, IIN);

				if (requestId == _photoRequestId) SetMemberPhoto(photo);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to load photo for member with IIN {IIN}", IIN);
				if (requestId == _photoRequestId) SetMemberPhoto(null);
			}
		}

		/// <summary>
		/// Replaces the shown photo (null shows the "No image" placeholder) and frees the previous image.
		/// </summary>
		private void SetMemberPhoto(byte[]? photo)
		{
			var previous = pictureBoxMember.Image;
			pictureBoxMember.Image = PictureHelper.ConvertByteToImage(photo);
			previous?.Dispose();
		}
	}
}