using Library.Application.Services.Repository;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
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
					DataGridViewForLendBook.DataSource = await _bookService.GetAllBooksAsync();
					break;

				case GridCriterion.BooksByMember:
					DataGridViewForLendBook.DataSource = await _memberService.GetMemberWithHisBooksAsync(IIN);
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

		private void UnlendABookToolStripMenuItem_Click(object sender, EventArgs e) //TODO Write
		{/*
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
			}*/
		}
	}
}
