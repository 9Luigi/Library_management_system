using Library.Data;
using Library.Services;
using Library.Models;
using Library.Data.Repositories;
using Library.Presentation.Controllers;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace Library
{
	public partial class FormBorrowOrReturnBook : Form
	{
		readonly ILogger _logger;
		readonly Repository<Book> _bookRepository;
		readonly Repository<Member> _memberRepository;
		readonly BookService _bookService;
		readonly MemberService _memberService;
		readonly CirculationService _libraryService;
		readonly LibraryContext _context = new(); //Disposed in desighner file
		private enum GridCriterion
		{
			AllBooks,
			BooksByMember
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string ActionMode { get; set; } = "BORROW";
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public long MemberIIN { get; set; }

		public FormBorrowOrReturnBook()
		{
			_logger = LoggerService.CreateLogger<FormBorrowOrReturnBook>();
			_bookRepository = new Repository<Book>();
			_memberRepository = new Repository<Member>();
			_bookService = new(_logger, _bookRepository);
			_memberService = new(_logger, _memberRepository, new LibraryContext());
			_libraryService = new();
			InitializeComponent();
		}

		private void FormBorrowOrReturnBook_Load(object sender, EventArgs e)
		{
			DataGridViewHelper.CustomizeDataGridView(this.DataGridViewForLendBook);

			InitializeForm();
		}

		private void InitializeForm()
		{
			switch (ActionMode.ToUpper())
			{
				case "BORROW":
					FillGridWith(GridCriterion.AllBooks);
					break;
				case "RETURN":
					FillGridWith(GridCriterion.BooksByMember);
					break;
			}
		}

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
					var memberBooks = await _memberService.GetMemberBooksAsync(_context, MemberIIN);
					if (memberBooks.Count == 0)
					{
						DataGridViewForLendBook.DataSource = null;
						MessageBoxHelper.ShowWarning($"No books found for member with IIN: {MemberIIN}.");
					}
					else
					{
						DataGridViewForLendBook.DataSource = memberBooks;
					}
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
				(bool isValid, int bookID) = DataGridViewHelper.TryGetValueFromRow<int>(DataGridViewForLendBook);
				if (isValid)
				{
					var result = await _libraryService.BorrowBook(bookID, MemberIIN);
					if (result) MessageBoxHelper.ShowSuccess("Book were succesfully borrowed");
					FillGridWith(GridCriterion.AllBooks);
				}
			}
			catch (InvalidOperationException ex)
			{
				ErrorHandler.HandleException(ex, ex.Message);
			}
			catch (Exception ex)
			{
				ErrorHandler.HandleException(ex, "An unexpected error occurred, contact system admin.");
			}
		}

		private async void ReturnBookToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				(bool isValid, int bookID) = DataGridViewHelper.TryGetValueFromRow<int>(DataGridViewForLendBook, 1);
				if (isValid)
				{
					var result = await _libraryService.ReturnBook(bookID, MemberIIN);
					if (result) MessageBoxHelper.ShowSuccess($"Book with ID {bookID} successfully returned.");
					FillGridWith(GridCriterion.BooksByMember);
				}
			}
			catch (InvalidOperationException ex)
			{
				ErrorHandler.HandleException(ex, ex.Message);
			}
			catch (Exception ex)
			{
				ErrorHandler.HandleException(ex, "An unexpected error occurred, contact system admin.");
			}
		}

		private void FormBorrowOrRecieveBook_FormClosed(object sender, FormClosedEventArgs e)
		{
			DataGridViewForLendBook.DataSource = null;
		}
	}
}