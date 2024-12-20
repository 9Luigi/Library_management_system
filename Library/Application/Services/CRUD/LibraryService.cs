using Library.Infrastructure.Repositories;
using Library.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Library.Presentation.Controllers;

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

		#region Helper
		/// <summary>
		/// Tries to retrieve a value from the selected row of the DataGridView.
		/// </summary>
		/// <typeparam name="T">The type of the value to retrieve.</typeparam>
		/// <param name="view">The DataGridView instance.</param>
		/// <param name="columnIndexToCheck">The index of the column to check (default is 0).</param>
		/// <returns>A tuple indicating whether the value was found and the value itself.</returns>
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

		/// <summary>
		/// Tries to parse a string value into a specified type.
		/// </summary>
		/// <typeparam name="T">The type to parse into.</typeparam>
		/// <param name="value">The string value to parse.</param>
		/// <param name="result">The parsed result.</param>
		/// <returns>True if parsing succeeded, otherwise false.</returns>
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

		/// <summary>
		/// Attempts to borrow a book for a member.
		/// </summary>
		/// <param name="bookID">The ID of the book to borrow.</param>
		/// <param name="IIN">The IIN of the member borrowing the book.</param>
		/// <returns>True if the book is successfully borrowed, otherwise false.</returns>
		internal async Task<bool> BorrowBook(int bookID, long IIN)
		{
			try
			{
				using var _dbContext = new LibraryContextForEFcore();
				_logger.LogInformation("Attempting to borrow book with ID: {BookID} for member with IIN: {IIN}", bookID, IIN);

				// Retrieve the book directly from the DbContext
				var selectedBook = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == bookID);

				// Validate book existence
				if (selectedBook == null)
				{
					_logger.LogWarning("Book with ID {BookID} does not exist", bookID);
					throw new InvalidOperationException($"Book with ID {bookID} does not exist.");
				}

				// Check if the book is available
				if (selectedBook.Amount <= 0)
				{
					_logger.LogWarning("Book {Title} has zero amount in store", selectedBook.Title);
					throw new InvalidOperationException($"Book '{selectedBook.Title}' has zero amount in store.");
				}

				// Retrieve the member directly from the DbContext, loading only the IIN and IDs of borrowed books
				var memberWithBookIds = await _dbContext.Members
					.Where(m => m.IIN == IIN)
					.Select(m => new
					{
						Member = m,
						BookIds = m.Books.Select(b => b.Id).ToList()
					})
					.FirstOrDefaultAsync();

				// Validate member existence
				if (memberWithBookIds == null)
				{
					_logger.LogWarning("Member with IIN {IIN} does not exist", IIN);
					throw new InvalidOperationException($"Member with IIN {IIN} does not exist.");
				}

				// Check if the member already borrowed this book
				if (memberWithBookIds.BookIds.Contains(bookID))
				{
					_logger.LogWarning("Member with IIN {IIN} has already borrowed book with ID {BookID}", IIN, bookID);
					throw new InvalidOperationException("This member has already borrowed this book.");
				}

				// Update the relationships and amounts
				memberWithBookIds.Member.Books.Add(selectedBook);
				selectedBook.Amount -= 1;

				// Save changes
				if (await _dbContext.SaveChangesAsync() > 0)
				{
					_logger.LogInformation("Book {Title} successfully borrowed by member with IIN {IIN}", selectedBook.Title, IIN);
					return true;
				}

				_logger.LogError("Failed to save changes for borrowing book with ID {BookID}", bookID);
				throw new InvalidOperationException("Failed to save changes.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while borrowing book with ID {BookID} for member with IIN {IIN}", bookID, IIN);
				throw; // Re-throw the exception to be handled in client code
			}
		}


		/// <summary>
		/// Attempts to return a borrowed book.
		/// </summary>
		/// <param name="bookID">The ID of the book to return.</param>
		/// <param name="IIN">The IIN of the member returning the book.</param>
		/// <returns>True if the book is successfully returned, otherwise false.</returns>
		internal async Task<bool> ReturnBook(int bookID, long IIN)
		{
			try
			{
				_logger.LogInformation("Attempting to return book with ID: {BookID} for member with IIN: {IIN}", bookID, IIN);

				using LibraryContextForEFcore context = new();

				// Retrieve the book to return
				var bookToReturn = await context.Books.FindAsync(bookID);
				if (bookToReturn == null)
				{
					_logger.LogWarning("Book with ID {BookID} not found", bookID);
					MessageBoxController.ShowWarning($"Book with ID {bookID} not found.");
					return false;
				}

				// Retrieve the member
				var memberWithBook = await context.Members
					.Include(m => m.Books) // Include borrowed books
					.SingleOrDefaultAsync(m => m.IIN == IIN);
				if (memberWithBook == null)
				{
					_logger.LogWarning("Member with IIN {IIN} not found", IIN);
					MessageBoxController.ShowWarning($"Member with IIN {IIN} not found.");
					return false;
				}

				// Ensure the member has borrowed the book
				bool hasBorrowedBook = memberWithBook.Books.Any(b => b.Id == bookID);
				if (!hasBorrowedBook)
				{
					_logger.LogWarning("Member with IIN {IIN} has not borrowed book with ID {BookID}", IIN, bookID);
					MessageBoxController.ShowWarning($"Member with IIN {IIN} has not borrowed book with ID {bookID}.");
					return false;
				}

				// Proceed with returning the book
				memberWithBook.Books.Remove(bookToReturn);
				bookToReturn.Amount += 1;

				// Save changes and log the result
				bool success = await context.SaveChangesAsync() > 0;
				if (success)
				{
					_logger.LogInformation("Book with ID {BookID} successfully returned by member with IIN {IIN}", bookID, IIN);
					MessageBoxController.ShowSuccess($"Book with ID {bookID} successfully returned.");
				}
				else
				{
					_logger.LogError("Failed to save changes for returning book with ID {BookID}", bookID);
					MessageBoxController.ShowError($"Failed to save changes for returning book with ID {bookID}. Please try again.");
				}

				return success;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while returning book with ID {BookID} for member with IIN {IIN}", bookID, IIN);
				MessageBoxController.ShowError($"An error occurred while returning the book: {ex.Message}");
				return false;
			}
		}

		#endregion
	}
}
