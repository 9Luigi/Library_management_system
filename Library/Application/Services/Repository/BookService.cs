using Library.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Library.Application.Services.Repository
{
	internal class BookService
	{
		internal readonly ILogger _logger;
		internal readonly GenericRepository<Book> _bookRepository;
		internal BookService(ILogger logger, GenericRepository<Book> bookRepository)
		{
			_logger = logger;
			_bookRepository = bookRepository;
		}

		internal async Task<List<dynamic>> GetAllBooksAsync()
		{
			_logger.LogInformation("GetMembersAsync started.");

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
				new LibraryContextForEFcore()
			);

			_logger.LogInformation("Retrieved {Count} members from the database.", books.Count);

			return books.Cast<dynamic>().ToList();
		}
	}
}
