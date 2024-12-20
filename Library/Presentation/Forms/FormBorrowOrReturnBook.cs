using Library.Application.Services.CRUD;
using Library.Domain.Models;
using Library.Infrastructure.Repositories;
using Library.Presentation.Controllers;
using Microsoft.Extensions.Logging;
using static Library.FormMembers;

namespace Library
{
	public partial class FormBorrowOrReturnBook : Form
	{
		readonly ILogger _logger;
		readonly GenericRepository<Book> _bookRepository;
		readonly GenericRepository<Member> _memberRepository;
		readonly BookService _bookService;
		readonly MemberService _memberService;
		readonly LibraryService _libraryService;

		private enum GridCriterion
		{
			AllBooks,
			BooksByMember
		}
		public FormBorrowOrReturnBook()
		{
			MemberCreateOrUpdateEvent += SelectBooksByIIN; //TODO create another event for this???
			_logger = LoggerService.CreateLogger<FormBorrowOrReturnBook>();
			_bookRepository = new GenericRepository<Book>();
			_memberRepository = new GenericRepository<Member>();
			_bookService = new(_logger, _bookRepository);
			_memberService = new(_logger, _memberRepository);
			_libraryService = new();
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

		private async void BorrowABookToolStripMenuItem_Click(object sender, EventArgs e) //TODO refresh grid
		{
			try
			{
				(bool isValid, int bookID) = LibraryService.TryGetValueFromRow<int>(DataGridViewForLendBook);
				if (isValid)
				{
					var result = await _libraryService.BorrowBook(bookID, IIN);
					if (result) MessageBoxController.ShowSuccess("Book were succesfully borrowed");
					FillGridWith(GridCriterion.AllBooks);
				}
			}
			catch (InvalidOperationException ex)
			{
				ErrorController.HandleException(ex, ex.Message);
			}
			catch (Exception ex)
			{
				ErrorController.HandleException(ex, "An unexpected error occurred, contact system admin.");
			}
		}

		private async void ReturnBookToolStripMenuItem_Click(object sender, EventArgs e) //TODO refresh grid
		{
			try
			{
				(bool isValid, int bookID) = LibraryService.TryGetValueFromRow<int>(DataGridViewForLendBook, 1); //1 is index of column with book id
				if (isValid)
				{
					await _libraryService.ReturnBook(bookID, IIN);
					FillGridWith(GridCriterion.BooksByMember);
				}
			}
			catch (InvalidOperationException ex)
			{
				ErrorController.HandleException(ex, "Check if member has this book");
			}
			catch (Exception ex)
			{
				ErrorController.HandleException(ex, "An unexpected error occurred, contact system admin.");
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
