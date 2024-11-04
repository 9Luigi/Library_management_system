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
using static Library.FMembers;

namespace Library
{
    public partial class FBorrowOrRecieveBook : Form
    {
        public FBorrowOrRecieveBook()
        {
            MemberCreateOrUpdateEvent += SelectBooksByIIN;
            InitializeComponent();
        }
        internal long IIN { get; private set; }
        private void FillGridWith(string criterion)
        {
            using LibraryContextForEFcore db = new();
            if (criterion == "ALL")
            {
                var selectedBooks = db.Books.Select
                        (b => new { b.Id, b.Title, b.Genre, b.Description, b.PublicationDate, b.Amount }).ToList();
                DataGridViewForLendBook.DataSource = selectedBooks;
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
                    DataGridViewForLendBook.DataSource = selectedBooks;
                }
            }
        }
        private void SelectBooksByIIN(MemberEventArgs e)
        {
            IIN = e.IIN;
            switch (e.Action)
            {
                case "BORROW":
                    FillGridWith("ALL");
                    break;
                case "RETURN":
                    FillGridWith(IIN.ToString());
                    break;
                default:
                    break;
            }
        }

        private void DataGridViewForLendBook_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridViewForLendBook.CurrentCell = DataGridViewForLendBook.Rows[e.RowIndex].Cells[e.ColumnIndex];
                Point relativeCursorPosition = DataGridViewForLendBook.PointToClient(Cursor.Position);
                CMSLendBook.Show(DataGridViewForLendBook, relativeCursorPosition);
            }
        }

        private void LendABookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DataGridViewForLendBook.CurrentCell.Value != null && DataGridViewForLendBook.CurrentCell.ColumnIndex == 0
                && long.TryParse(DataGridViewForLendBook.CurrentCell.Value.ToString(), out long ID))
            {
                using LibraryContextForEFcore db = new();
                var selectedBook = db.Books.FirstOrDefault(b => b.Id == ID);
                var selectedBooks = db.Books.Select(b => new { b.Id, b.Title, b.Genre, b.Description, b.PublicationDate, b.Amount }).ToList();
                if (selectedBook!.Amount > 0)
                {
                    var selectedMember = db.Members.FirstOrDefault(m => m.IIN == IIN);
                    selectedMember!.Books.Add(selectedBook);
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
        private void UnlendABookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DataGridViewForLendBook.CurrentCell.Value != null && DataGridViewForLendBook.CurrentCell.ColumnIndex == 0
                && long.TryParse(DataGridViewForLendBook.CurrentCell.Value.ToString(), out long ID)) //TODO null or ""?
            {
                using LibraryContextForEFcore db = new();
                var selectedMember = db.Members.Include(m => m.Books).FirstOrDefault(m => m.IIN == IIN);
                var selectedBook = db.Books.FirstOrDefault(b => b.Id == ID);
                selectedMember!.Books.Remove(selectedBook!);
                selectedBook!.Amount += 1;
                if (db.SaveChanges() > 0)
                {
                    MessageBox.Show($"{selectedBook.Title} succesfully return by {selectedMember.Name} {selectedMember.Surname}");
                }
            }
        }
    }
}
