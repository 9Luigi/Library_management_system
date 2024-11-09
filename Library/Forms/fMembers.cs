using Library.Controllers;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Library
{
	public partial class FMembers : Form
	{
		public FMembers()
		{
			InitializeComponent();
			FlendOrRecieveBook = new FBorrowOrRecieveBook();
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

		internal FBorrowOrRecieveBook FlendOrRecieveBook { get; private set; }
		internal FaddEdit_prop FaddEdit_prop { get; private set; }
		internal long IIN { get; set; }
		internal CancellationTokenSource? CancellationTokenSource { get; set; }
		internal CancellationToken CancellationToken { get; set; }
		internal delegate void MemberCreateOrUpdateDelegate(MemberEventArgs e);
		static internal event MemberCreateOrUpdateDelegate? MemberCreateOrUpdateEvent;
		private void AddMemberToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//Transfer data to FaddEdit_prop form, subscribed to MemberCreateOrUpdateEvent on FaddEdit_prop constructor
			MemberCreateOrUpdateEvent!.Invoke(new MemberEventArgs("CREATE"));
			FaddEdit_prop.ShowDialog();
			RefreshDataGridForMembers();

		}
		private void FMembers_Load(object sender, EventArgs e)
		{
			RefreshDataGridForMembers();
		}
		private void EditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(bool b, long i) = IsIIN_Clicked(IIN);
			if (b)
			{
				MemberCreateOrUpdateEvent!.Invoke(new MemberEventArgs("EDIT", i));
				FaddEdit_prop.ShowDialog();
				RefreshDataGridForMembers();
			}
		}

		private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(bool b, long i) = IsIIN_Clicked(IIN);
			if (b)
			{
				Task deleteMember = new TaskFactory().StartNew(new Action(() =>
				{
					using LibraryContextForEFcore db = new();
					Member? memberToDelete = db.Members.FirstOrDefault(m => m.IIN == i);
					db.Members.Remove(memberToDelete!);
					DialogResult result = MessageBox.Show("Are you sure to remove?", "Removing member", MessageBoxButtons.YesNo);
					if (result == DialogResult.Yes)
					{
						if (db.SaveChanges() > 0)
						{
							RefreshDataGridForMembers();
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

		private void view_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && e.RowIndex > -1 && e.ColumnIndex == 0)
			{
				dataGridViewForMembers.CurrentCell = dataGridViewForMembers.Rows[e.RowIndex].Cells[e.ColumnIndex];
				//TODO click to column name == Exception
				Point relativeCursorPosition = dataGridViewForMembers.PointToClient(Cursor.Position);
				cmMember.Show(dataGridViewForMembers, relativeCursorPosition);
				//if clicked by right button cell is IIN then popup menu executes
			}
		}

		private void TbIINSearch_TextChanged(object sender, EventArgs e) //TODO logic when lenght <3
		{
			using LibraryContextForEFcore db = new();
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
			}
		}
		private void RefreshDataGridForMembers() //TODO maybe better don't close connection after each operation?
		{
			int totalMembersCount;
			//TODO maybe better use AsNotTracking
			ControlsEnableFlag(false); //while data from db is loading all controls enabled set to false
			CancellationTokenSource = new CancellationTokenSource();
			CancellationToken = CancellationTokenSource.Token;
			Task fillGridWithAllMembers = new TaskFactory().StartNew(new Action(async () => //TODO async+await instead of Task and CancelationToken
				{
					using (LibraryContextForEFcore db = new())
					{

						totalMembersCount = db.Members.Count(); //recieve members count //TODO for correct progress bar refresh in future

						if (CancellationToken.IsCancellationRequested) { return; }
						await ProgressBarController.pbProgressCgange(this, pbMembers, 0, 25);

						//this.Invoke(ProgressBarController.pbProgressCgange, this, pbMembers, 0, 25); //TODO check if searched by IIN
						var users = db.Members.Include(m => m.Books).Select(m => new //TODO handle exception of select and login to db or db not create
						{
							m.IIN,
							m.Name,
							m.Surname,
							m.Age,
							Books = string.Join(", ", m.Books.Select(b => b.Title)),
						}).ToList();
						if (CancellationToken.IsCancellationRequested) { return; }
						await ProgressBarController.pbProgressCgange(this, pbMembers,25, 50);
						this.Invoke(new Action(() =>
						{
							dataGridViewForMembers.DataSource = users; //TODO error catch or logic to avoid/ avoid what? null?
							DataGridViewController.CustomizeDataGridView(dataGridViewForMembers);
						}));
						if (CancellationToken.IsCancellationRequested) { return; }
						await ProgressBarController.pbProgressCgange(this, pbMembers, 50, 100);
					}
					Thread.Sleep(500);

					if (CancellationToken.IsCancellationRequested) { return; }
					if (pbMembers.IsDisposed) { return; }

					this.Invoke(ProgressBarController.pbProgressReset, pbMembers);
					this.Invoke(ControlsEnableFlag, true); // after data load from db set all controls enabled true
				}), CancellationToken); //TODO all invoke call exception if form isdisposed earlier than invokable method
		}
		void ControlsEnableFlag(bool flag) //TODO change name by more suitable
										   //Set all controls enabled property according to flag
		{
			foreach (Control item in this.Controls)
			{
				this.Invoke(new Action(() =>
				{
					item.Enabled = flag;
				}));
			}
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

		private void LeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(bool b, long i) = IsIIN_Clicked(IIN);
			if (b)
			{
				MemberCreateOrUpdateEvent!.Invoke(new MemberEventArgs("BORROW", i));
				FlendOrRecieveBook.ShowDialog();
				RefreshDataGridForMembers();
			}
		}

		private void SeeLendedBooksForThisMemberToolStripMenuItem_Click(object sender, EventArgs e)//borrowed books*
		{
			//this method checked selected member for borrowed books and if true sends data to another form
			(bool b, long i) = IsIIN_Clicked(IIN);
			if (b)
			{
				using LibraryContextForEFcore db = new();
				Member? selectedMemberName = db.Members.Where(m => m.IIN == i).Select(m => new Member()
				{
					Name = m.Name,
					Surname = m.Surname
				}).FirstOrDefault();
				var selectedBooks = db.Members.Where(m => m.IIN == i)
					.Include(m => m.Books).SelectMany(m => m.Books.Select(b => new
					{
						b.Id,
						b.Title,
						b.Genre
					})).ToList();
				if (selectedBooks.Count > 0)
				{
					MemberCreateOrUpdateEvent!.Invoke(new MemberEventArgs("RETURN", i));
					FlendOrRecieveBook.ShowDialog();
					RefreshDataGridForMembers();
				}
				else
				{
					MessageBox.Show($"{selectedMemberName!.Name} {selectedMemberName!.Surname} don't borrowed yet");
				}
			}
		}//borrowed books
		(bool, long) IsIIN_Clicked(long IIN) //check data grid first column for IIN value and if true return tuple 
											 //TODO check for null 
											 //TODO refactor via index(click on row not only IIN cell for edit)
		{
			if (dataGridViewForMembers.CurrentCell.Value != null && dataGridViewForMembers.CurrentCell.ColumnIndex == 0
				&& long.TryParse(dataGridViewForMembers.CurrentCell.Value.ToString(), out IIN))
			{
				return (true, IIN);
			}
			else return (false, 0);
		}
	}
}
