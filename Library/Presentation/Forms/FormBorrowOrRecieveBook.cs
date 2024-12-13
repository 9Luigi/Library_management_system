using Library.Application.Services.Repository;
using Library.Domain.Models;
using Library.Presentation.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Library.FormMembers;

namespace Library
{
	public partial class FormBorrowOrRecieveBook : Form
	{
		ILogger _logger;
		GenericRepository<Book> _bookRepository;
		GenericRepository<Member> _memberRepository;
		BookService _bookService;
		MemberService _memberService;
		private enum GridCriterion
		{
			AllBooks,
			BooksByMember
		}
		public FormBorrowOrRecieveBook()
		{
			MemberCreateOrUpdateEvent += SelectBooksByIIN;
			_logger = LoggerService.CreateLogger<FormBorrowOrRecieveBook>();
			_bookRepository = new GenericRepository<Book>();
			_memberRepository = new GenericRepository<Member>();
			_bookService = new(_logger, _bookRepository);
			_memberService = new(_logger, _memberRepository);
			InitializeComponent();
		}
		internal long IIN { get; private set; }
		private async void FillGridWith(GridCriterion criterion)
		{
			switch (criterion)
			{
				case GridCriterion.AllBooks:
					returnBookToolStripMenuItem.Visible = false;
					lendABookToolStripMenuItem.Visible = true;
					DataGridViewForLendBook.DataSource = await _bookService.GetAllBooksAsync();
					break;

				case GridCriterion.BooksByMember:
					returnBookToolStripMenuItem.Visible = true;
					lendABookToolStripMenuItem.Visible = false;
					await _memberService.GetMemberWithHisBooksToDataGridViewAsync(IIN, DataGridViewForLendBook);
					break;
			}
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

		private async void BorrowABookToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				//TODO Handle click on any cell except headers
				if (DataGridViewForLendBook.CurrentCell?.Value != null && DataGridViewForLendBook.CurrentCell.ColumnIndex == 0)
				{
					if (int.TryParse(DataGridViewForLendBook.CurrentCell.Value.ToString(), out int bookId))
					{
						using LibraryContextForEFcore db = new();
						var selectedBook = await db.Books.FirstOrDefaultAsync(b => b.Id == bookId); //TODO Use service
						var memberWithBooks = await db.Members
						.Include(m => m.Books)
						.FirstOrDefaultAsync(m => m.IIN == IIN); //TODO Use service

						// check if book and member exist
						if (selectedBook == null) { MessageBox.Show("Selected book does not exist"); return; }
						if (memberWithBooks == null) { MessageBox.Show("Selected member does not exist"); return; }
						if (selectedBook.Amount <= 0) { MessageBox.Show($"{selectedBook.Title} has zero amount in store"); return; }
						if (memberWithBooks.Books.Any(b => b.Id == bookId))
						{
							MessageBox.Show("This member has already borrowed this book.");
							return;
						}

						// Add book to member
						memberWithBooks.Books.Add(selectedBook);
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
			catch (Exception ex) { MessageBox.Show($"An error occurred: {ex.Message}"); }
		}

		private void ReturnBookToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Ensure a row is selected
			if (DataGridViewForLendBook.CurrentRow == null)
			{
				MessageBox.Show("No row selected in the DataGridView");
				return;
			}

			// Find the "BookID" column
			var bookIdColumn = DataGridViewForLendBook.Columns["BookID"];
			if (bookIdColumn == null)
			{
				MessageBox.Show("Column 'BookID' not found in the DataGridView");
				return;
			}

			// Retrieve the value from the "BookID" column in the current row
			var cellValue = DataGridViewForLendBook.CurrentRow.Cells[bookIdColumn.Index]?.Value;
			if (cellValue == null || !long.TryParse(cellValue.ToString(), out long bookId))
			{
				MessageBox.Show("Invalid Book ID");
				return;
			}

			using LibraryContextForEFcore db = new();
			var selectedMember = db.Members.Include(m => m.Books).FirstOrDefault(m => m.IIN == IIN);
			if (selectedMember == null)
			{
				MessageBox.Show("Member not found");
				return;
			}

			var selectedBook = db.Books.FirstOrDefault(b => b.Id == bookId);
			if (selectedBook == null)
			{
				MessageBox.Show("Book not found");
				return;
			}

			// Remove the book from the member's book list
			selectedMember.Books.Remove(selectedBook);
			selectedBook.Amount += 1;

			// Save changes to the database
			if (db.SaveChanges() > 0)
			{
				MessageBox.Show($"{selectedBook.Title} successfully returned by {selectedMember.Name} {selectedMember.Surname}");
				// Refresh the DataGridView after returning the book
				FillGridWith(GridCriterion.BooksByMember);
			}
		}


		private void FormBorrowOrRecieveBook_FormClosed(object sender, FormClosedEventArgs e)
		{
			DataGridViewForLendBook.DataSource = null;
		}

		private void FormBorrowOrRecieveBook_Load(object sender, EventArgs e)
		{
			DataGridViewController.CustomizeDataGridView(this.DataGridViewForLendBook);
		}
	}
}
