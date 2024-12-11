using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Library
{
	public class MemberService
	{
		internal readonly ILogger _logger;
		internal readonly GenericRepository<Member> _memberRepository;

		/// <summary>
		/// Initializes a new instance of the <see cref="MemberService"/> class.
		/// </summary>
		/// <param name="logger">The logger to log messages related to member operations.</param>
		/// <param name="memberRepository">The repository used to access member data in the database.</param>
		internal MemberService(ILogger logger, GenericRepository<Member> memberRepository)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
		}

		/// <summary>
		/// Retrieves a list of members with their details, including a list of borrowed books.
		/// </summary>
		/// <returns>A list of dynamic objects representing the members and their borrowed books.</returns>
		/// <exception cref="Exception">Thrown when an error occurs while retrieving member data from the database.</exception>
		public async Task<List<dynamic>> GetMembersAsync()
		{
			try
			{
				_logger.LogInformation("GetMembersAsync started.");

				var members = await _memberRepository.GetCollectionWithProjectionAsync(
					m => new
					{
						m.IIN,
						m.Name,
						m.Surname,
						m.Age,
						m.RegistrationDate,
						Books = string.Join(", ", m.Books.Select(b => b.Title))
					},
					new LibraryContextForEFcore(),
					m => m.Books
				);

				_logger.LogInformation("Retrieved {Count} members from the database.", members);

				return members.Cast<dynamic>().ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while retrieving members from the database.");
				throw;
			}
		}

		/// <summary>
		/// Retrieves a specific member's data along with a list of borrowed books based on the provided IIN.
		/// </summary>
		/// <param name="IIN">The IIN (Identification Number) of the member to retrieve.</param>
		/// <returns>A dynamic object representing the member's data and borrowed books, or null if the member is not found.</returns>
		/// <exception cref="Exception">Thrown when an error occurs while retrieving member data from the database.</exception>
		internal async Task<Member> GetMemberWithBorrowedBooksAsync(long IIN)
		{
			try
			{
				// Log entry into the method
				_logger.LogInformation("Attempting to retrieve member data and borrowed books for IIN: {IIN}", IIN);

				// Define the projection to retrieve member data along with their borrowed books
				var member = await _memberRepository.GetOneWithProjectionAsync(
					m => m,
					new LibraryContextForEFcore(),
					m => m.Books // Include the Books navigation property
				);

				// Log the success of data retrieval
				_logger.LogInformation("Successfully retrieved member data and borrowed books for IIN: {IIN}", IIN);

				// Log the result of the filtering
				if (member != null)
				{
					_logger.LogInformation("Member found for IIN: {IIN}", IIN);
				}
				else
				{
					_logger.LogWarning("No member found for IIN: {IIN}", IIN);
					throw new KeyNotFoundException($"Entity with IIN: {IIN} not found");
				}

				return member;
			}
			catch (Exception ex)
			{
				// Log the exception with relevant details
				_logger.LogError(ex, "An error occurred while fetching member data and borrowed books for IIN: {IIN}", IIN);
				throw; // Re-throw the exception after logging it
			}
		}
		/// <summary>
		/// Retrieves a list of books for the specified member based on their IIN.
		/// </summary>
		/// <param name="memberIIN">The IIN of the member for whom books are to be retrieved.</param>
		/// <returns>Member with included books</returns>
		/// <exception cref="KeyNotFoundException">Thrown when no books are found for the specified member.</exception>
		internal async Task<List<dynamic>> GetMemberWithHisBooksAsync(long memberIIN)
		{
			_logger.LogInformation("Start fetching books for member with IIN: {MemberIIN}", memberIIN);

			using var context = new LibraryContextForEFcore();

			var memberWithBooks = await context.Members
				.Include(m => m.Books)
				.Where(m => m.IIN == memberIIN)
				.FirstOrDefaultAsync();

			if (memberWithBooks == null || memberWithBooks.Books == null || !memberWithBooks.Books.Any())
			{
				_logger.LogWarning("No books found for member with IIN: {MemberIIN}", memberIIN);
				throw new KeyNotFoundException($"Books for member with IIN {memberIIN} not found.");
			}

			_logger.LogInformation("Successfully retrieved {BookCount} books for member with IIN: {MemberIIN}", memberWithBooks.Books.Count, memberIIN);

		 var booksData = memberWithBooks.Books.Select(b => new //TODO transfer Member, not anonym
			{
				BookId = b.Id,
				BookTitle = b.Title,
			}).ToList<dynamic>();
			return booksData;
		}
	}
}