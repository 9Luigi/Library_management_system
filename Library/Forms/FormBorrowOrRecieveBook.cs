using Microsoft.EntityFrameworkCore;
using System.Data;
using static Library.FormMembers;

namespace Library
{
	public partial class FormBorrowOrRecieveBook : Form
	{
		private enum GridCriterion
		{
			AllBooks,
			BooksByMember
		}
		public FormBorrowOrRecieveBook()
		{
			MemberCreateOrUpdateEvent += SelectBooksByIIN;
			InitializeComponent();
		}
		internal long IIN { get; private set; }
		private async void FillGridWith(GridCriterion criterion)
		{
			using LibraryContextForEFcore db = new();
			switch (criterion)
			{
				case GridCriterion.AllBooks:
					DataGridViewForLendBook.DataSource = await GetAllBooksAsync(db);
					break;

				case GridCriterion.BooksByMember:
					DataGridViewForLendBook.DataSource = await GetBooksByMemberAsync(db, IIN);
					break;
			}
		}

		private async Task<List<dynamic>> GetAllBooksAsync(LibraryContextForEFcore db)
		{
			return await db.Books
				.Select(b => new
				{
					b.Id,
					b.Title,
					b.Genre,
					b.Description,
					b.PublicationDate,
					b.Amount
				}).Cast<dynamic>()
				.ToListAsync();
		}

		private async Task<List<dynamic>> GetBooksByMemberAsync(LibraryContextForEFcore db, long memberIIN)
		{
			return await db.Members
				.Where(m => m.IIN == memberIIN)
				.SelectMany(m => m.Books.Select(b => new
				{
					b.Id,
					b.Title,
					b.Genre
				})).Cast<dynamic>()
				.ToListAsync();
		}

		//TODO make toolstrip enabled false or something like that than to example deny acces to return strip when action is borrow
		private void SelectBooksByIIN(MemberEventArgs e)
		{
			IIN = e.IIN;
			switch (e.Action)
			{
				case "BORROW":
					FillGridWith(GridCriterion.AllBooks);
					break;
				case "RETURN":
					FillGridWith(GridCriterion.BooksByMember);
					break;
				default:
					break;
			}
		}

		private void DataGridViewForLendBook_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0) // Check than cell is not header
			{
				DataGridViewForLendBook.CurrentCell = DataGridViewForLendBook.Rows[e.RowIndex].Cells[e.ColumnIndex];
				Point relativeCursorPosition = DataGridViewForLendBook.PointToClient(Cursor.Position);
				CMSLendBook.Show(DataGridViewForLendBook, relativeCursorPosition);
			}
		}

		private async void LendABookToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				//TODO Handle click on any cell except headers
				if (DataGridViewForLendBook.CurrentCell?.Value != null && DataGridViewForLendBook.CurrentCell.ColumnIndex == 0)
				{
					if (int.TryParse(DataGridViewForLendBook.CurrentCell.Value.ToString(), out int bookId))
					{
						using LibraryContextForEFcore db = new();
						var selectedBook = await db.Books.FirstOrDefaultAsync(b => b.Id == bookId);
						var selectedMemberBooks = await db.Members.Where(m=>m.IIN == IIN).SelectMany(m=>m.Books).ToListAsync();

						// check if book and member exist
						if (selectedBook == null) { MessageBox.Show("Selected book does not exist"); return; }
						if (selectedMemberBooks == null) { MessageBox.Show("Selected member does not exist"); return; }
						if (selectedBook.Amount <= 0) { MessageBox.Show($"{selectedBook.Title} has zero amount in store"); return; }
						if (selectedMemberBooks.Any(b => b.Id == bookId))
						{
							MessageBox.Show("This member has already borrowed this book.");
							return;
						}

						// Add book to member
						selectedMemberBooks.Add(selectedBook);
						// Decrease book amount
						selectedBook.Amount -= 1;

						if (await db.SaveChangesAsync() > 0)
						{
							MessageBox.Show("Book successfully borrowed");
							FillGridWith(GridCriterion.AllBooks);
						}
					}
				}
			}
			catch(Exception ex) { MessageBox.Show($"An error occurred: {ex.Message}"); }
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
						FillGridWith(GridCriterion.BooksByMember); //FillGrid seems than be called only when ivent from fmembers is triggered
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
