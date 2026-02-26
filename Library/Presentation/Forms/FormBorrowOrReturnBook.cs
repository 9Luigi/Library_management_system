using Library.Application.Services.CRUD;
using Library.Domain.Models;
using Library.Infrastructure.Repositories;
using Library.Presentation.Controllers;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

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
		readonly LibraryContextForEFcore _context = new(); //Disposed in desighner file
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
			_bookRepository = new GenericRepository<Book>();
			_memberRepository = new GenericRepository<Member>();
			_bookService = new(_logger, _bookRepository);
			_memberService = new(_logger, _memberRepository, new LibraryContextForEFcore());
			_libraryService = new();
			InitializeComponent();
		}

		private void FormBorrowOrReturnBook_Load(object sender, EventArgs e)
		{
			DataGridViewController.CustomizeDataGridView(this.DataGridViewForLendBook);

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
					await _memberService.GetMemberWithHisBooksToDataGridViewAsync(_context, MemberIIN, DataGridViewForLendBook);
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
				(bool isValid, int bookID) = LibraryService.TryGetValueFromRow<int>(DataGridViewForLendBook);
				if (isValid)
				{
					var result = await _libraryService.BorrowBook(bookID, MemberIIN);
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

		private async void ReturnBookToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				(bool isValid, int bookID) = LibraryService.TryGetValueFromRow<int>(DataGridViewForLendBook, 1);
				if (isValid)
				{
					await _libraryService.ReturnBook(bookID, MemberIIN);
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
	}
}