using Library.Infrastructure.Repositories;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Library.Application.Services.CRUD
{
	internal class LibraryService
	{
		private readonly GenericRepository<Member> _memberRepository;
		private readonly GenericRepository<Book> _bookRepository;
		private readonly ILogger _logger;

		public LibraryService()
		{
			_bookRepository = new GenericRepository<Book>();
			_memberRepository = new GenericRepository<Member>();
			_logger = LoggerService.CreateLogger<LibraryService>();
		}
		#region Validation
		internal static (bool, T) TryGetValueFromRow<T>(DataGridView view, int columnIndexToCheck = 0) where T : struct
		{
			// Check if any cell is selected
			if (view.CurrentCell == null)
				return (false, default);

			// Get the row index of the selected cell and ensure it is valid
			int rowIndex = view.CurrentCell.RowIndex;
			if (rowIndex < 0 || rowIndex >= view.Rows.Count)
				return (false, default);

			// Ensure the specified column index is valid
			if (columnIndexToCheck < 0 || columnIndexToCheck >= view.ColumnCount)
				return (false, default);

			// Check the value in the specified column of the selected row
			var cellValue = view.Rows[rowIndex].Cells[columnIndexToCheck]?.Value;
			if (cellValue != null && TryParseValue(cellValue.ToString()!, out T value))
			{
				return (true, value);
			}

			return (false, default);
		}

		private static bool TryParseValue<T>(string value, out T result) where T : struct
		{
			if (typeof(T) == typeof(long))
			{
				if (long.TryParse(value, out var longResult))
				{
					result = (T)(object)longResult;
					return true;
				}
			}
			else if (typeof(T) == typeof(int))
			{
				if (int.TryParse(value, out var intResult))
				{
					result = (T)(object)intResult;
					return true;
				}
			}

			result = default;
			return false;
		}

		#endregion
		#region CRUD
		internal async Task<bool> BorrowBook(int bookID, long IIN)
		{
			try
			{
				using var _dbContext = new LibraryContextForEFcore();
				_logger.LogInformation("Attempting to borrow book with ID: {BookID} for member with IIN: {IIN}", bookID, IIN);

				// Retrieve the book using the generic repository
				var selectedBook = await _bookRepository.GetByIdAsync(_dbContext, bookID);

				// Retrieve the member with books using the generic repository
				var memberWithBooks = await _memberRepository.GetByFieldAsync(
					_dbContext,
					m => m.IIN,
					IIN
				);

				// Include books explicitly if required (manual loading for navigation property)
				_dbContext.Entry(memberWithBooks)
						  .Collection(m => m.Books)
						  .Load();

				// Validate book existence
				if (selectedBook == null)
				{
					_logger.LogWarning("Book with ID {BookID} does not exist", bookID);
					throw new InvalidOperationException("Selected book does not exist");
				}

				// Validate member existence
				if (memberWithBooks == null)
				{
					_logger.LogWarning("Member with IIN {IIN} does not exist", IIN);
					throw new InvalidOperationException("Selected member does not exist");
				}

				// Check if the book is available
				if (selectedBook.Amount <= 0)
				{
					_logger.LogWarning("Book {Title} has zero amount in store", selectedBook.Title);
					throw new InvalidOperationException($"{selectedBook.Title} has zero amount in store");
				}

				// Check if the member already borrowed this book
				if (memberWithBooks.Books.Any(b => b.Id == bookID))
				{
					_logger.LogWarning("Member with IIN {IIN} has already borrowed book with ID {BookID}", IIN, bookID);
					throw new InvalidOperationException("This member has already borrowed this book.");
				}

				// Update data
				memberWithBooks.Books.Add(selectedBook);
				selectedBook.Amount -= 1;

				// Save changes
				if (await _dbContext.SaveChangesAsync() > 0)
				{
					_logger.LogInformation("Book {Title} successfully borrowed by member with IIN {IIN}", selectedBook.Title, IIN);
					return true;
				}

				_logger.LogError("Failed to save changes for borrowing book with ID {BookID}", bookID);
				throw new InvalidOperationException("Failed to save changes");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while borrowing book with ID {BookID} for member with IIN {IIN}", bookID, IIN);
				throw; // Re-throw the exception to be handled in client code
			}
		}

		#endregion
	}
}
