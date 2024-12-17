using Library.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Library.Domain.Models;
namespace Library.Application.Services.CRUD
{
	/// <summary>
	/// Service class responsible for operations related to books.
	/// </summary>
	internal class BookService
	{
		private readonly ILogger _logger;
		private readonly GenericRepository<Book> _bookRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="BookService"/> class.
		/// </summary>
		/// <param name="logger">Logger instance to log information, warnings, and errors.</param>
		/// <param name="bookRepository">Repository for accessing book data.</param>
		internal BookService(ILogger logger, GenericRepository<Book> bookRepository)
		{
			_logger = logger;
			_bookRepository = bookRepository;
		}

		#region CRUD

		/// <summary>
		/// Retrieves a list of all books with selected properties from the database.
		/// </summary>
		/// <returns>A list of books with title, genre, description, publication date, and amount.</returns>
		/// <exception cref="Exception">Throws exception if an error occurs while fetching data.</exception>
		internal async Task<List<dynamic>> GetAllBooksAsync()
		{
			using LibraryContextForEFcore dbContext = new();
			_logger.LogInformation("GetAllBooksAsync started.");

			try
			{
				var books = await _bookRepository.GetCollectionWithProjectionAsync(
					b => new
					{
						b.Id,
						b.Title,
						b.Genre,
						b.Description,
						b.PublicationDate,
						b.Amount
					},
					dbContext
				);

				_logger.LogInformation("Retrieved {Count} books from the database.", books.Count);

				return books.Cast<dynamic>().ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while retrieving books from the database.");
				throw;
			}
		}

		/// <summary>
		/// Retrieves a single book by its ID from the database.
		/// </summary>
		/// <param name="id">The ID of the book to retrieve.</param>
		/// <returns>A <see cref="Book"/> object if found, otherwise throws an exception.</returns>
		/// <exception cref="KeyNotFoundException">Thrown when the book with the specified ID does not exist.</exception>
		internal async Task<Book> GetBookAsync(int id)
		{
			using LibraryContextForEFcore dbContext = new();
			try
			{
				var book = await _bookRepository.GetByIdAsync(dbContext, id);

				if (book == null)
				{
					_logger.LogWarning("No book found with id: {Id} in the database.", id);
					throw new KeyNotFoundException($"Book with id {id} not found.");
				}

				_logger.LogInformation("Retrieved book with id: {Id}, Title: {Title} from the database.", book.Id, book.Title);
				return book;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while retrieving book with id: {Id}.", id);
				throw;
			}
		}
		#endregion
	}
}
