using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Library.fMembers;

namespace Library
{
    public partial class fLendOrRecieveBook : Form
    {
        public fLendOrRecieveBook()
        {
            MemberCreateOrUpdateEvent += selectBooksByIIN;
            InitializeComponent();
        }
        internal long IIN { get; private set; }
        private void fLendOrRecieveBook_Load(object sender, EventArgs e)
        {

        }
        private void fillGridWith(string criterion)
        {
            using (LibraryContextForEFcore db = new LibraryContextForEFcore())
            {
                if (criterion == "ALL")
                {
                    var selectedBooks = db.Books.Select
                            (b => new { b.Id, b.Title, b.Genre, b.Description, b.PublicationDate, b.Amount }).ToList();
                    dataGridViewForLendBook.DataSource = selectedBooks;
                }
                else
                {
                    var selectedBooks = db.Members.Where(m => m.IIN == IIN)
                        .Include(m => m.Books).SelectMany(m => m.Books.Select(b => new
                        {
                            b.Id,
                            b.Title,
                            b.Genre
                        })).ToList();
                    if (selectedBooks.Count > 0)
                    {
                        dataGridViewForLendBook.DataSource = selectedBooks;
                    }
                }
            }
        }
        private void selectBooksByIIN(MemberEventArgs e)
        {
            IIN = e.IIN;
            switch (e.Action)
            {
                case "BORROW":
                    fillGridWith("ALL");
                    break;
                case "RETURN":
                    fillGridWith(IIN.ToString());
                    break;
                default:
                    break;
            }
        }

        private void dataGridViewForLendBook_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dataGridViewForLendBook.CurrentCell = dataGridViewForLendBook.Rows[e.RowIndex].Cells[e.ColumnIndex];
                Point relativeCursorPosition = dataGridViewForLendBook.PointToClient(Cursor.Position);
                cmsLendBook.Show(dataGridViewForLendBook, relativeCursorPosition);
            }
        }

        private void lendABookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long ID;
            if (dataGridViewForLendBook.CurrentCell.Value != null && dataGridViewForLendBook.CurrentCell.ColumnIndex == 0
                && long.TryParse(dataGridViewForLendBook.CurrentCell.Value.ToString(), out ID))
            {
                using (LibraryContextForEFcore db = new LibraryContextForEFcore())
                {
                    var selectedBook = db.Books.FirstOrDefault(b => b.Id == ID);
                    var selectedBooks = db.Books.Select(b => new { b.Id, b.Title, b.Genre, b.Description, b.PublicationDate, b.Amount }).ToList();
                    if (selectedBook.Amount > 0)
                    {
                        var selectedMember = db.Members.FirstOrDefault(m => m.IIN == IIN);
                        selectedMember.Books.Add(selectedBook);
                        selectedBook.Amount -= 1;
                        try
                        {
                            if (db.SaveChanges() > 0)
                            {
                                MessageBox.Show("Book succesfully borrowed");
                            }
                        }
                        catch (DbUpdateException)
                        {
                            MessageBox.Show("Same book were already borrowed to this member");
                        }
                    }
                    else MessageBox.Show("Cannot borrowed the book when it's amount is 0");
                }
            }
        }
        private void unlendABookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long ID;
            if (dataGridViewForLendBook.CurrentCell.Value != null && dataGridViewForLendBook.CurrentCell.ColumnIndex == 0
                && long.TryParse(dataGridViewForLendBook.CurrentCell.Value.ToString(), out ID)) //TODO null or ""?
            {
                using (LibraryContextForEFcore db = new LibraryContextForEFcore())
                {
                    var selectedMember = db.Members.Include(m => m.Books).FirstOrDefault(m => m.IIN == IIN);
                    var selectedBook = db.Books.FirstOrDefault(b => b.Id == ID);
                    selectedMember!.Books.Remove(selectedBook);
                    selectedBook.Amount += 1;
                    if (db.SaveChanges() > 0)
                    {
                        MessageBox.Show($"{selectedBook.Title} succesfully return by {selectedMember.Name} {selectedMember.Surname}");
                    }
                }
            }
        }
    }
}
