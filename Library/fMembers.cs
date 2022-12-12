using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    public partial class fMembers : Form
    {
        public fMembers()
        {
            InitializeComponent();
        }
        internal class MemberEventArgs : EventArgs
        {
            internal long IIN { get; private set; }
            internal string Action { get; private set; }
            public MemberEventArgs(string action, long IIN = 0)
            {
                this.IIN = IIN;
                Action = action;
            }
        }
        internal CancellationTokenSource cancellationTokenSource { get; set; }
        internal CancellationToken cancellationToken { get; set; }
        internal delegate void OnfAddEditCreatedDelegate(MemberEventArgs e);
        static internal event OnfAddEditCreatedDelegate OnfAddEditCreatedEvent;
        private void addMemberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (LibraryContextForEFcore db = new LibraryContextForEFcore()) //TODO create a method!
            {
                fAddEdit dade = new fAddEdit();
                OnfAddEditCreatedEvent.Invoke(new MemberEventArgs("CREATE"));
                dade.ShowDialog();
                RefreshDataGridForMembers();
            }
        }
        private void fMembers_Load(object sender, EventArgs e)
        {
            RefreshDataGridForMembers();
        }
        public void pbProgressCgange(ProgressBar pb, int startValue, int finalValue)
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
        public void pbProgressReset(ProgressBar pb)
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

        private async void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long IIN;
            if (dataGridViewForMembers.CurrentCell.Value != null && dataGridViewForMembers.CurrentCell.ColumnIndex == 0
                && long.TryParse(dataGridViewForMembers.CurrentCell.Value.ToString(), out IIN)) //TODO create a method?
            {
                using (LibraryContextForEFcore db = new LibraryContextForEFcore())
                {
                    var members = await db.Members.Where(m => m.IIN == IIN).ToListAsync();
                    fAddEdit dade = new fAddEdit();
                    OnfAddEditCreatedEvent.Invoke(new MemberEventArgs("EDIT", IIN));
                    dade.ShowDialog();
                    RefreshDataGridForMembers();
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long IIN;
            if (dataGridViewForMembers.CurrentCell.Value != null && dataGridViewForMembers.CurrentCell.ColumnIndex == 0
                && long.TryParse(dataGridViewForMembers.CurrentCell.Value.ToString(), out IIN))
            {
                Task deleteMember = new TaskFactory().StartNew(new Action(() =>
                {
                    using (LibraryContextForEFcore db = new LibraryContextForEFcore())
                    {
                        Member memberToDelete = db.Members.FirstOrDefault(m => m.IIN == Convert.ToInt64(IIN));
                        db.Members.Attach(memberToDelete);
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
            if (e.Button == MouseButtons.Right)
            {
                this.dataGridViewForMembers.CurrentCell = this.dataGridViewForMembers.Rows[e.RowIndex].Cells[e.ColumnIndex];
                this.cmMember.Show(this.dataGridViewForMembers, new Point(e.RowIndex, e.ColumnIndex));
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
            controlsEnableFlag(false);
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            Task fillGridWithAllMembers = new TaskFactory().StartNew(new Action(() =>
                {
                    using (LibraryContextForEFcore db = new LibraryContextForEFcore())
                    {
                        if (cancellationToken.IsCancellationRequested) { return; }
                        this.Invoke(pbProgressCgange, pbMembers, 0, 25); //TODO check if searched by IIN
                        var users = db.Members.Include(m=>m.Books).Select(m => new
                        {
                            m.IIN,
                            m.Name,
                            m.Surname,
                            m.Age,
                            m.Books
                        }).ToList();
                        if (cancellationToken.IsCancellationRequested) { return; }
                        this.Invoke(pbProgressCgange, pbMembers, 25, 85);
                        if (cancellationToken.IsCancellationRequested) { return; }
                        this.Invoke(new Action(() =>
                        {
                            dataGridViewForMembers.DataSource = users; //TODO error catch or logic to avoid
                        }));
                        if (cancellationToken.IsCancellationRequested) { return; }
                        this.Invoke(pbProgressCgange, pbMembers, 50, 100);
                    }
                    Thread.Sleep(500);
                    if (cancellationToken.IsCancellationRequested) { return; }
                    this.Invoke(pbProgressReset, pbMembers);
                    if (cancellationToken.IsCancellationRequested) { return; }
                    this.Invoke(controlsEnableFlag, true);
                }), cancellationToken); //TODO all invoke call exception if form isdisposed earlier than invokable method
        }
        void controlsEnableFlag(bool flag)
        {
            foreach (Control item in this.Controls)
            {
                item.Enabled = flag;
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void ifFormDisposedThanTaskKill(Form form, Task task)
        {
            if (form.IsDisposed)
            {

            }
        }
        private void fMembers_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancellationTokenSource.Cancel();
        }
        private void TbIINSearch_Click(object sender, EventArgs e)
        {
            TbIINSearch.Text = "";
        }
    }
}
