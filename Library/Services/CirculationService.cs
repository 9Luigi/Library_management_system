using Library.Data;
using Library.Data.Repositories;
using Library.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
	internal class CirculationService
	{
		private readonly Repository<Member> _memberRepository;
		private readonly Repository<Book> _bookRepository;
		private readonly ILogger _logger;

		public CirculationService()
		{
			_bookRepository = new Repository<Book>();
			_memberRepository = new Repository<Member>();
			_logger = LoggerService.CreateLogger<CirculationService>();
		}

		#region CRUD

		/// <summary>
		/// Borrows a book for a member.
		/// </summary>
		/// <param name="bookID">The ID of the book to borrow.</param>
		/// <param name="IIN">The IIN of the member borrowing the book.</param>
		/// <returns>True if the book is successfully borrowed.</returns>
		/// <exception cref="InvalidOperationException">Book/member not found, no copies left, already borrowed or nothing saved.</exception>
		internal async Task<bool> BorrowBook(int bookID, long IIN)
		{
			try
			{
				using var _dbContext = new LibraryContext();
				_logger.LogInformation("Attempting to borrow book with ID: {BookID} for member with IIN: {IIN}", bookID, IIN);

				// Tracking is required here: the context is configured with NoTracking by default
				var selectedBook = await _dbContext.Books.AsTracking().FirstOrDefaultAsync(b => b.Id == bookID);

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

				// Retrieve the member together with the IDs of borrowed books
				var memberWithBookIds = await _dbContext.Members
					.AsTracking()
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
		/// Returns a borrowed book.
		/// </summary>
		/// <param name="bookID">The ID of the book to return.</param>
		/// <param name="IIN">The IIN of the member returning the book.</param>
		/// <returns>True if the book is successfully returned.</returns>
		/// <exception cref="InvalidOperationException">Book/member not found, book not borrowed by this member or nothing saved.</exception>
		internal async Task<bool> ReturnBook(int bookID, long IIN)
		{
			try
			{
				_logger.LogInformation("Attempting to return book with ID: {BookID} for member with IIN: {IIN}", bookID, IIN);

				using LibraryContext context = new();

				// Tracking is required here: the context is configured with NoTracking by default
				var bookToReturn = await context.Books.AsTracking().FirstOrDefaultAsync(b => b.Id == bookID);
				if (bookToReturn == null)
				{
					_logger.LogWarning("Book with ID {BookID} not found", bookID);
					throw new InvalidOperationException($"Book with ID {bookID} not found.");
				}

				var memberWithBook = await context.Members
					.AsTracking()
					.Include(m => m.Books) // Include borrowed books
					.SingleOrDefaultAsync(m => m.IIN == IIN);
				if (memberWithBook == null)
				{
					_logger.LogWarning("Member with IIN {IIN} not found", IIN);
					throw new InvalidOperationException($"Member with IIN {IIN} not found.");
				}

				// Ensure the member has borrowed the book
				var borrowedBook = memberWithBook.Books.FirstOrDefault(b => b.Id == bookID);
				if (borrowedBook == null)
				{
					_logger.LogWarning("Member with IIN {IIN} has not borrowed book with ID {BookID}", IIN, bookID);
					throw new InvalidOperationException($"Member with IIN {IIN} has not borrowed book with ID {bookID}.");
				}

				memberWithBook.Books.Remove(borrowedBook);
				bookToReturn.Amount += 1;

				if (await context.SaveChangesAsync() > 0)
				{
					_logger.LogInformation("Book with ID {BookID} successfully returned by member with IIN {IIN}", bookID, IIN);
					return true;
				}

				_logger.LogError("Failed to save changes for returning book with ID {BookID}", bookID);
				throw new InvalidOperationException("Failed to save changes.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while returning book with ID {BookID} for member with IIN {IIN}", bookID, IIN);
				throw;
			}
		}

		#endregion
	}
}
