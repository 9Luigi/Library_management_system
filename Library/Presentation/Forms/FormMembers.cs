using Library.Api;
using Library.Services;
using Library.Presentation.Controllers;
using Library.Presentation.Theme;
using Microsoft.Extensions.Logging;
namespace Library
{
	// TODO paging, search by IIN, member photo, borrow/return books (see the desktop-sqlite branch)
	public partial class FormMembers : Form
	{
		#region FieldsAndProperties
		readonly ILogger _logger;

		public FormMembers()
		{
			InitializeComponent();
			AppTheme.Apply(this);
			_logger = LoggerService.CreateLogger<FormMembers>();
		}
		#endregion

		private async void AddMemberToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using FaddEdit_prop _FaddEdit_prop = new();
			_FaddEdit_prop.ActionMode = "CREATE";
			_FaddEdit_prop.ShowDialog();

			await RefreshDataGridForMembers();
		}

		private async void FMembers_Load(object sender, EventArgs e)
		{
			// Embedded into the main window: closing is handled there
			exitToolStripMenuItem.Visible = TopLevel;

			await RefreshDataGridForMembers();
		}

		private async void EditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(bool isValid, string IIN) = DataGridViewHelper.TryGetValueFromRow(dataGridViewForMembers);
			if (isValid) await EditMemberAsync(IIN);
		}

		private async void DataGridViewForMembers_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left || e.RowIndex < 0 || e.ColumnIndex < 0) return;

			dataGridViewForMembers.CurrentCell = dataGridViewForMembers.Rows[e.RowIndex].Cells[e.ColumnIndex];

			var (isValid, IIN) = DataGridViewHelper.TryGetValueFromRow(dataGridViewForMembers);
			if (isValid) await EditMemberAsync(IIN);
		}

		private async Task EditMemberAsync(string IIN)
		{
			using FaddEdit_prop _FaddEdit_prop = new();
			_FaddEdit_prop.ActionMode = "UPDATE";
			_FaddEdit_prop.MemberIIN = IIN;
			_FaddEdit_prop.ShowDialog();

			await RefreshDataGridForMembers();
		}

		private async void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(bool isValid, string IIN) = DataGridViewHelper.TryGetValueFromRow(dataGridViewForMembers);
			if (!isValid)
			{
				MessageBoxHelper.ShowWarning("Please select a member to proceed.");
				return;
			}

			if (!MessageBoxHelper.ShowConfirmation("Are you sure to remove?", "Removing member")) return;

			try
			{
				await ServerConnection.CallAsync(() => MembersApi.DeleteAsync(IIN));
				MessageBoxHelper.ShowSuccess("Member successfully removed");
			}
			catch (ApiException ex)
			{
				ErrorHandler.HandleException(ex, ex.Message);
			}

			await RefreshDataGridForMembers();
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

		private async Task RefreshDataGridForMembers()
		{
			try
			{
				dataGridViewForMembers.DataSource = await ServerConnection.CallAsync(MembersApi.GetAllAsync);
				DataGridViewHelper.CustomizeDataGridView(dataGridViewForMembers);
			}
			catch (ApiException ex)
			{
				ErrorHandler.HandleException(ex, ex.Message);
			}
		}

		private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void FMembers_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing &&
				!MessageBoxHelper.ShowConfirmation("Are you sure you want to exit?", "Confirm Exit"))
			{
				e.Cancel = true;
			}
		}
	}
}
