using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Library
{
    public partial class fMembers : Form
    {
        public fMembers()
        {
            InitializeComponent();
            flendOrRecieveBook = new fLendOrRecieveBook();
            faddEdit = new fAddEdit();
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
        internal fLendOrRecieveBook flendOrRecieveBook { get; private set; }
        internal fAddEdit faddEdit { get; private set; }
        internal long IIN { get; set; }
        internal CancellationTokenSource cancellationTokenSource { get; set; }
        internal CancellationToken cancellationToken { get; set; }
        internal delegate void MemberCreateOrUpdateDelegate(MemberEventArgs e);
        static internal event MemberCreateOrUpdateDelegate? MemberCreateOrUpdateEvent;
        private void addMemberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemberCreateOrUpdateEvent!.Invoke(new MemberEventArgs("CREATE"));
            faddEdit.ShowDialog();
            RefreshDataGridForMembers();
        }
        private void fMembers_Load(object sender, EventArgs e)
        {
            RefreshDataGridForMembers();
        }
        internal void pbProgressCgange(ProgressBar pb, int startValue, int finalValue)
        {
            pb.Visible = true;
            this.Invoke(new Action(() =>
            {
                for (int i = startValue; i < finalValue; i++)
                {
                    pb.PerformStep();
                    Task.Delay(100);
                }
            }));
        }
        internal void pbProgressReset(ProgressBar pb)
        {
            pb.Invoke(new Action(() =>
            {
                if (pb.Value == 100)
                {
                    pb.Value = 0;
                    pb.Visible = false;
                }
            }));
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (bool b, long i) = isIIN_Clicked(IIN);
            if (b)
            {
                MemberCreateOrUpdateEvent.Invoke(new MemberEventArgs("EDIT", i));
                faddEdit.ShowDialog();
                RefreshDataGridForMembers();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (bool b, long i) = isIIN_Clicked(IIN);
            if (b)
            {
                Task deleteMember = new TaskFactory().StartNew(new Action(() =>
                {
                    using (LibraryContextForEFcore db = new LibraryContextForEFcore())
                    {
                        Member? memberToDelete = db.Members.FirstOrDefault(m => m.IIN == i);
                        db.Members.Remove(memberToDelete);
                        DialogResult result = MessageBox.Show("Are you sure to remove?", "Removing member", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            if (db.SaveChanges() > 0)
                            {
                                RefreshDataGridForMembers();
                                MessageBox.Show("Member succesfully removed");
                            }
                        }
                    }
                }));
            }
            else
            {
                MessageBox.Show("Cannot delete this member, try later or communicate your system admin");
            }
        }

        private void dataGridViewForMembers_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex>-1 && e.ColumnIndex==0)
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
            using (LibraryContextForEFcore db = new LibraryContextForEFcore())
            {
                if (TbIINSearch.Text.Length > 3)
                {

                    long IIN;
                    long.TryParse(TbIINSearch.Text, out IIN);
                    if (IIN != 0)
                    {
                        var MatchedMembers = db.Members.Where(m => EF.Functions.Like(m.IIN.ToString(), $"%{IIN.ToString()}%")).
                            Select(m => new { m.IIN, m.Name, m.Surname, m.Age }).ToList();
                        dataGridViewForMembers.DataSource = MatchedMembers; //TODO something
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
        }
        private void RefreshDataGridForMembers() //TODO maybe better don't close connection after each operation?
        {
            //TODO maybe better use AsNotTracking
            controlsEnableFlag(false); //while data from db is loading all controls enabled set to false
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            Task fillGridWithAllMembers = new TaskFactory().StartNew(new Action(() =>
                {
                    using (LibraryContextForEFcore db = new LibraryContextForEFcore())
                    {
                        if (cancellationToken.IsCancellationRequested) { return; }
                        this.Invoke(ProgressBarController.pbProgressCgange, this, pbMembers, 0, 25); //TODO check if searched by IIN
                        var users = db.Members.Select(m => new
                        {
                            m.IIN,
                            m.Name,
                            m.Surname,
                            m.Age,
                        }).ToList();
                        if (cancellationToken.IsCancellationRequested) { return; }
                        this.Invoke(ProgressBarController.pbProgressCgange, this, pbMembers, 25, 85);
                        if (cancellationToken.IsCancellationRequested) { return; }
                        this.Invoke(new Action(() =>
                        {
                            dataGridViewForMembers.DataSource = users; //TODO error catch or logic to avoid
                        }));
                        if (cancellationToken.IsCancellationRequested) { return; }
                        this.Invoke(ProgressBarController.pbProgressCgange, this, pbMembers, 50, 100);
                    }
                    Thread.Sleep(500);
                    if (cancellationToken.IsCancellationRequested) { return; }
                    this.Invoke(ProgressBarController.pbProgressReset, pbMembers);
                    if (cancellationToken.IsCancellationRequested) { return; }
                    this.Invoke(controlsEnableFlag, true); // after data load from db set all controls enabled true
                }), cancellationToken); //TODO all invoke call exception if form isdisposed earlier than invokable method
        }
        void controlsEnableFlag(bool flag)
        //Set all controls enabled property according to flag
        {
            foreach (Control item in this.Controls)
            {
                this.Invoke(new Action(()=>{
                    item.Enabled = flag;
                })); 
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void fMembers_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancellationTokenSource.Cancel(); //most likely bad arhitecture but allow to avoid error(this.invoke after this closed)
        }
        private void TbIINSearch_Click(object sender, EventArgs e)
        {
            TbIINSearch.Text = "";
        }

        private void leToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (bool b, long i) = isIIN_Clicked(IIN);
            if (b)
            {
                MemberCreateOrUpdateEvent!.Invoke(new MemberEventArgs("BORROW", i));
                flendOrRecieveBook.ShowDialog();
                RefreshDataGridForMembers();
            }
        }

        private void seeLendedBooksForThisMemberToolStripMenuItem_Click(object sender, EventArgs e)//borrowed books*
        {
            //this method checked selected member for borrowed books and if true sends data to another form
           (bool b, long i) = isIIN_Clicked(IIN);
            if (b)
            {
                using (LibraryContextForEFcore db = new LibraryContextForEFcore())
                {
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
                        flendOrRecieveBook.ShowDialog();
                        RefreshDataGridForMembers();
                    }
                    else
                    {
                        MessageBox.Show($"{selectedMemberName!.Name} {selectedMemberName!.Surname} don't borrowed yet");
                    }
                }
            }
        }//borrowed books
        (bool,long) isIIN_Clicked(long IIN) //check data grid first column for IIN value and if true return tuple
        {
            if (dataGridViewForMembers.CurrentCell.Value != null && dataGridViewForMembers.CurrentCell.ColumnIndex == 0
                && long.TryParse(dataGridViewForMembers.CurrentCell.Value.ToString(), out IIN))
            {
                return (true,IIN);
            }
            else return (false,0);
            
        }
    }
}
