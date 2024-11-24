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
using static Library.FormMembers;

namespace Library
{
	public partial class FormBorrowOrRecieveBook : Form
	{
		public FormBorrowOrRecieveBook()
		{
			MemberCreateOrUpdateEvent += SelectBooksByIIN;
			InitializeComponent();
		}
		internal long IIN { get; private set; }
		private void FillGridWith(string criterion) //TODO change criterion type to enum 
		{
			using LibraryContextForEFcore db = new();
			if (criterion == "ALL")
			{
				var selectedBooks = db.Books
					.Select(b => new
					{
						b.Id,
						b.Title,
						b.Genre,
						b.Description,
						b.PublicationDate,
						b.Amount
					})
					.ToList();
				DataGridViewForLendBook.DataSource = selectedBooks;
			}
			else
			{
				var selectedBooks = db.Members
					.Where(m => m.IIN == IIN)
					.Include(m => m.Books)
					.SelectMany(m => m.Books.Select(b => new
					{
						b.Id,
						b.Title,
						b.Genre
					}))
					.ToList();

				if (selectedBooks.Any()) // Null check
				{
					DataGridViewForLendBook.DataSource = selectedBooks;
				}
				else
				{
					MessageBox.Show("No books found for this member.");
					Close();
				}
			}
		}
		//TODO make toolstrip enabled false or something like that than to example deny acces to return strip when action is borrow
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
			if (e.Button == MouseButtons.Right && e.RowIndex >= 0) // Check than cell is not header
			{
				DataGridViewForLendBook.CurrentCell = DataGridViewForLendBook.Rows[e.RowIndex].Cells[e.ColumnIndex];
				Point relativeCursorPosition = DataGridViewForLendBook.PointToClient(Cursor.Position);
				CMSLendBook.Show(DataGridViewForLendBook, relativeCursorPosition);
			}
		}


		private void LendABookToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (DataGridViewForLendBook.CurrentCell?.Value != null && DataGridViewForLendBook.CurrentCell.ColumnIndex == 0)
			{
				if (long.TryParse(DataGridViewForLendBook.CurrentCell.Value.ToString(), out long bookId))
				{
					using LibraryContextForEFcore db = new();
					var selectedBook = db.Books.FirstOrDefault(b => b.Id == bookId);
					var selectedMember = db.Members.FirstOrDefault(m => m.IIN == IIN);

					// check if book and member exist
					if (selectedBook != null && selectedMember != null)
					{
						if (selectedBook.Amount > 0)
						{
							// Add book to member
							selectedBook.Amount -= 1;
							selectedMember.Books.Add(selectedBook);
							FillGridWith("All");  //FillGrid seems than be called only when ivent from fmembers is triggered
												  //TODO Update DataGridView binding by updating the data source for amount cell only
							var selectedBooks = db.Books
								.Select(b => new
								{
									b.Id,
									b.Title,
									b.Genre,
									b.Description,
									b.PublicationDate,
									b.Amount
								}).ToList();
							DataGridViewForLendBook.DataSource = selectedBooks;

							try
							{
								if (db.SaveChanges() > 0)
								{
									MessageBox.Show("Book successfully borrowed");
								}
							}
							catch (DbUpdateException)
							{
								MessageBox.Show("This book has already been borrowed by this member");
							}
						}
						else
						{
							MessageBox.Show("Cannot borrow the book when its amount is 0");
						}
					}
					else
					{
						MessageBox.Show("Selected book or member not found");
					}
				}
				else
				{
					MessageBox.Show("Invalid book ID");
				}
			}
		}

		private void UnlendABookToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (DataGridViewForLendBook.CurrentCell?.Value != null && DataGridViewForLendBook.CurrentCell.ColumnIndex == 0)
			{
				if (long.TryParse(DataGridViewForLendBook.CurrentCell.Value.ToString(), out long bookId))
				{
					using LibraryContextForEFcore db = new();
					var selectedMember = db.Members.Include(m => m.Books).FirstOrDefault(m => m.IIN == IIN);
					var selectedBook = db.Books.FirstOrDefault(b => b.Id == bookId);

					// check if book and member exist
					if (selectedMember != null && selectedBook != null)
					{
						// Remove book from member
						selectedMember.Books.Remove(selectedBook);
						selectedBook.Amount += 1; //TODO refresh grid after borrow a book
						FillGridWith(IIN.ToString()); //FillGrid seems than be called only when ivent from fmembers is triggered
						// Update DataGridView binding by updating the data source
						var selectedBooks = db.Members
								   .Where(m => m.IIN == IIN)
								   .Include(m => m.Books)
								   .SelectMany(m => m.Books.Select(b => new
								   {
									   b.Id,
									   b.Title,
									   b.Genre
								   }))
								   .ToList();
						DataGridViewForLendBook.DataSource = selectedBooks;

						if (db.SaveChanges() > 0)
						{
							MessageBox.Show($"{selectedBook.Title} successfully returned by {selectedMember.Name} {selectedMember.Surname}");

						}
					}
					else
					{
						MessageBox.Show("Selected book or member not found");
					}
				}
				else
				{
					MessageBox.Show("Invalid book ID");
				}
			}
		}
	}
}
