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

        private async void viewAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (  LibraryContextForEFcore db = new LibraryContextForEFcore())
            {
                var members = await db.Members.Select(m => new { m.IIN,m.Name,m.Surname,m.PhoneNumber}).ToListAsync();

                dataGridViewForMembers.DataSource =  members;
            }
        }
        private void dataGridViewForMembers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewForMembers.CurrentCell.ColumnIndex.Equals(0))
            {
                if (dataGridViewForMembers.CurrentCell != null && dataGridViewForMembers.CurrentCell.Value != null)
                    MessageBox.Show(dataGridViewForMembers.CurrentCell.Value.ToString());
            }
        }
        private void TbIINSearch_MouseDown(object sender, MouseEventArgs e)
        {
            TbIINSearch.Text = "";
        }

        private void TbIINSearch_Leave(object sender, EventArgs e)
        {
            
        }

        private void bIINSearch_Click(object sender, EventArgs e)
        {
            using (LibraryContextForEFcore db = new LibraryContextForEFcore())
            {
                long IIN = Convert.ToInt64(TbIINSearch.Text);
                var selectedUser = db.Members.Where(m => m.IIN == IIN).Select(m => new
                {
                    m.IIN,
                    m.Name,
                    m.Surname,
                    m.Age
                }).ToList();
                dataGridViewForMembers.DataSource = selectedUser;
            }
        }
        private void editOneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void addMemberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAddDeleteEdit fadddeleteedit = new fAddDeleteEdit();
            fadddeleteedit.ShowDialog();
        }
    }
}
