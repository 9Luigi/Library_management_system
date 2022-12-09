using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            internal long IIN { get; set; }
            public MemberEventArgs(long IIN)
            {
                this.IIN = IIN;
            }
        }
        internal delegate void OnfAddDeleteEditCreatedDelegate(MemberEventArgs e);
        static internal event OnfAddDeleteEditCreatedDelegate OnfAddDeleteEditCreatedEvent;
        private void bIINSearch_Click(object sender, EventArgs e)
        {
            Task fillGridWithSelectedMember = new TaskFactory().StartNew(new Action(() =>
            {
                using (LibraryContextForEFcore db = new LibraryContextForEFcore())
                {
                    long number;
                    if (long.TryParse(TbIINSearch.Text, out number))
                    {
                        long IIN = number;
                        this.Invoke(pbProgressCgange, pbMembers, 0, 25);
                        var selectedUser = db.Members.Where(m => m.IIN == IIN).Select(m => new //change where to firstOrDefault
                        {
                            m.IIN,
                            m.Name,
                            m.Surname,
                            m.Age
                        }).ToList();
                        this.Invoke(pbProgressCgange, pbMembers, 25, 50);
                        this.Invoke(new Action(() =>
                        {
                            dataGridViewForMembers.DataSource = selectedUser; //TODO error catch or logic to avoid
                        }));
                        this.Invoke(pbProgressCgange, pbMembers, 50, 100);
                    }
                }
                Thread.Sleep(500);
                this.Invoke(pbProgressReset, pbMembers);
            }));
        }

        private void addMemberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAddDeleteEdit dade = new fAddDeleteEdit();
            dade.ShowDialog();
        }

        private async void dataGridViewForMembers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewForMembers.CurrentCell.Value != null && dataGridViewForMembers.CurrentCell.ColumnIndex == 0)
            {
                long IIN = Convert.ToInt64(dataGridViewForMembers.CurrentCell.Value);
                using (LibraryContextForEFcore db = new LibraryContextForEFcore())
                {
                    var members = await db.Members.Where(m => m.IIN == IIN).ToListAsync();
                    fAddDeleteEdit dade = new fAddDeleteEdit();
                    OnfAddDeleteEditCreatedEvent.Invoke(new MemberEventArgs(IIN));
                    dade.ShowDialog();
                }
            }
        }

        private void fMembers_Load(object sender, EventArgs e)
        {
            Task fillGridWithAllMembers = new TaskFactory().StartNew(new Action(() =>
            {
                using (LibraryContextForEFcore db = new LibraryContextForEFcore())
                {
                    this.Invoke(pbProgressCgange, pbMembers, 0, 25);
                    var users = db.Members.Select(m => new
                    {
                        m.IIN,
                        m.Name,
                        m.Surname,
                        m.Age
                    }).ToList();
                    this.Invoke(pbProgressCgange, pbMembers, 25, 85);
                    this.Invoke(new Action(() =>
                    {
                        dataGridViewForMembers.DataSource = users; //TODO error catch or logic to avoid
                    }));
                    this.Invoke(pbProgressCgange, pbMembers, 50, 100);
                }
                Thread.Sleep(500);
                this.Invoke(pbProgressReset, pbMembers);
            }));

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
                    fAddDeleteEdit dade = new fAddDeleteEdit();
                    OnfAddDeleteEditCreatedEvent.Invoke(new MemberEventArgs(IIN));
                    dade.ShowDialog();
                    dataGridViewForMembers.Update();
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
                                MessageBox.Show("Member succesfully removed");
                            }
                        }
                    }
                    this.Invoke(new Action(() =>
                    {
                        dataGridViewForMembers.Update();
                    }));
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
    }
}
