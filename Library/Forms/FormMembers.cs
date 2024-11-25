using Library.Controllers;
using Library.Models;
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

		private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(bool b, long i) = DataGridViewController.TryGetIINFromRow(dataGridViewForMembers);
			if (b)
			{
				Task deleteMember = new TaskFactory().StartNew(new Action(async () =>
			   {
				   using LibraryContextForEFcore db = new();
				   Member? memberToDelete = db.Members.FirstOrDefault(m => m.IIN == i);
				   db.Members.Remove(memberToDelete!);
				   DialogResult result = MessageBox.Show("Are you sure to remove?", "Removing member", MessageBoxButtons.YesNo);
				   if (result == DialogResult.Yes)
				   {
					   if (db.SaveChanges() > 0)
					   {
						   await RefreshDataGridForMembers();
						   MessageBox.Show("Member succesfully removed");
					   }
				   }
			   }));
			}
			else
			{
				MessageBox.Show("Cannot delete this member, try later or communicate your system admin");
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

		private async Task RefreshDataGridForMembers()
		{
			// Disable controls while data from DB is loading
			await SetControlsEnabledAsync(false);

			CancellationToken = CancellationTokenSource.Token;

			Task fillGridWithAllMembers = new TaskFactory().StartNew(new Action(async () =>
			{
				using (LibraryContextForEFcore db = new())
				{
					int totalMembersCount = await GetTotalMembersCountAsync(db);

					if (CancellationToken.IsCancellationRequested) return;

					await UpdateProgressBarAsync(0, 25);

					var users = await GetMembersFromDbAsync(db);

					if (CancellationToken.IsCancellationRequested) return;

					await UpdateProgressBarAsync(25, 50);

					await UpdateDataGridViewAsync(users);

					if (CancellationToken.IsCancellationRequested) return;

					await UpdateProgressBarAsync(50, 100);
				}

				Thread.Sleep(500);

				if (CancellationToken.IsCancellationRequested) return;

				if (pbMembers.IsDisposed) return;

				// Reset the progress bar and enable controls
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

			// Преобразуем анонимный тип в dynamic
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
		private void TbIINSearch_Click(object sender, EventArgs e)
		{
			TbIINSearch.Text = "";
		}

		private async void LeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(bool b, long i) = DataGridViewController.TryGetIINFromRow(dataGridViewForMembers);
			if (b)
			{
				MemberCreateOrUpdateEvent!.Invoke(new MemberEventArgs("BORROW", i));
				FlendOrRecieveBook.ShowDialog();
				await RefreshDataGridForMembers();
			}
		}

		private async void SeeLendedBooksForThisMemberToolStripMenuItem_Click(object sender, EventArgs e)//borrowed books*
		{
			//this method checked selected member for borrowed books and if true sends data to another form
			(bool b, long i) = DataGridViewController.TryGetIINFromRow(dataGridViewForMembers);
			if (b)
			{
				using LibraryContextForEFcore db = new();
				Member? selectedMemberName = db.Members.Where(m => m.IIN == i).Select(m => new Member()
				{
					Name = m.Name,
					Surname = m.Surname
				}).FirstOrDefault();
				var selectedBooks = await db.Members.Where(m => m.IIN == i)
					.Include(m => m.Books).SelectMany(m => m.Books.Select(b => new
					{
						b.Id,
						b.Title,
						b.Genre
					})).ToListAsync();
				if (selectedBooks.Count > 0)
				{
					MemberCreateOrUpdateEvent!.Invoke(new MemberEventArgs("RETURN", i));
					FlendOrRecieveBook.ShowDialog();
					await RefreshDataGridForMembers();
				}
				else
				{
					MessageBox.Show($"{selectedMemberName!.Name} {selectedMemberName!.Surname} don't borrowed yet");
				}
			}
		}//borrowed books
	}
}
