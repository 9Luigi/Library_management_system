using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Library.Infrastructure.Repositories;
using Library.Presentation.Controllers;
using System;

namespace Library.Application.Services.CRUD
{
	/// <summary>
	/// Service class responsible for operations related to library members.
	/// </summary>
	public class MemberService
	{
		private readonly ILogger _logger;
		private readonly GenericRepository<Member> _memberRepository;

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
		#region CRUD

		/// <summary>
		/// Retrieves a list of members filtered by their IIN, including their borrowed books.
		/// </summary>
		/// <param name="IIN">The IIN of the member to filter by.</param>
		/// <returns>
		/// A task that represents the asynchronous operation. The task result contains a list of dynamic objects
		/// with the following properties:
		/// - IIN: The IIN of the member.
		/// - Name: The name of the member.
		/// - Surname: The surname of the member.
		/// - Age: The age of the member.
		/// - RegistrationDate: The registration date of the member.
		/// - Books: A string containing the titles of the books borrowed by the member, separated by commas.
		/// </returns>
		/// <exception cref="Exception">
		/// Thrown if there is an error during data retrieval or processing.
		/// </exception>
		internal async Task<List<dynamic>> GetFilteredMembersAsync(long IIN)
		{
			using LibraryContextForEFcore dbContext = new();
			try
			{
				var results = await _memberRepository.GetCollectionWithProjectionAsync(
					m => new
					{
						m.IIN,
						m.Name,
						m.Surname,
						m.Age,
						m.RegistrationDate,
						Books = m.Books.Select(b => b.Title)
					},
					IIN,
					m => m.IIN,
					dbContext,
					m => m.Books
				);

				// Projection to dynamic
				return results.Select(member => (dynamic)new
				{
					member.IIN,
					member.Name,
					member.Surname,
					member.Age,
					member.RegistrationDate,
					Books = string.Join(", ", member.Books)
				}).ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while fetching filtered members.");
				throw;
			}
		}

		/// <summary>
		/// Retrieves a list of members with their information, including borrowed book titles.
		/// </summary>
		/// <param name="dbContext">The <see cref="LibraryContextForEFcore"/> instance for database operations.</param>
		/// <returns>
		/// A task that represents the asynchronous operation. The task result contains a list of dynamic objects
		/// with the following properties:
		/// - IIN: The IIN of the member.
		/// - Name: The name of the member.
		/// - Surname: The surname of the member.
		/// - Age: The age of the member.
		/// - RegistrationDate: The registration date of the member.
		/// - Books: A string containing the titles of the books borrowed by the member, separated by commas.
		/// </returns>
		/// <exception cref="Exception">
		/// Thrown if there is an error during data retrieval.
		/// </exception>
		internal async Task<List<dynamic>> GetMembersAsync()
		{
			using LibraryContextForEFcore dbContext = new();
			try
			{
				_logger.LogInformation("GetMembersAsync started.");

				// Retrieve members with their borrowed books
				var members = await _memberRepository.GetCollectionWithProjectionAsync(
					m => new
					{
						m.IIN,
						m.Name,
						m.Surname,
						m.Age,
						m.RegistrationDate,
						Books = string.Join(", ", m.Books.Select(b => b.Title)) // Join book titles
					},
					dbContext,
					m => m.Books
				);

				_logger.LogInformation("Retrieved {Count} members from the database.", members.Count);

				// Return the members as dynamic objects
				return members.Cast<dynamic>().ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while retrieving members from the database.");
				throw;
			}
		}

		/// <summary>
		/// Asynchronously retrieves the count of borrowed books for a member based on their IIN.
		/// </summary>
		/// <param name="IIN">The IIN (Individual Identification Number) of the member whose borrowed books count is to be retrieved.</param>
		/// <returns>Returns an integer representing the number of borrowed books. If no member is found, returns 0.</returns>
		/// <exception cref="Exception">Throws an exception if an error occurs while querying the database.</exception>
		public async Task<int> GetBorrowedBooksCountAsync(long IIN)
		{
			using (var context = new LibraryContextForEFcore())
			{
				return await context.Members
					.Where(m => m.IIN == IIN)
					.Select(m => m.Books.Count)  // Get the count of borrowed books
					.FirstOrDefaultAsync();
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
			using LibraryContextForEFcore dbContext = new();
			try
			{
				// Log entry into the method
				_logger.LogInformation("Attempting to retrieve member data and borrowed books for IIN: {IIN}", IIN);

				// Define the projection to retrieve member data along with their borrowed books
				var member = await _memberRepository.GetOneByIDWithProjectionAsync(
					m => m,
					dbContext,
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
		/// Retrieves a member with their books by IIN and populates the given DataGridView with the retrieved data.
		/// </summary>
		/// <param name="memberIIN">The IIN of the member to fetch.</param>
		/// <param name="dataGridView">The DataGridView control to display the member's books.</param>
		/// <returns>
		/// Returns the <see cref="Member"/> object containing the member's details and their books.
		/// </returns>
		/// <exception cref="KeyNotFoundException">
		/// Thrown when the member or their books cannot be found in the database.
		/// </exception>
		/// <remarks>
		/// This method populates the DataGridView with the books related to the member and updates the 
		/// top-left header cell of the DataGridView with the member's details.
		/// </remarks>
		internal async Task<bool> GetMemberWithHisBooksToDataGridViewAsync(long memberIIN, DataGridView dataGridView)
		{
			using LibraryContextForEFcore dbContext = new();
			_logger.LogInformation("Start fetching books for member with IIN: {MemberIIN}", memberIIN);

			var memberWithBooks = await dbContext.Members
				.Include(m => m.Books)
				.Where(m => m.IIN == memberIIN)
				.FirstOrDefaultAsync();

			if (memberWithBooks == null || memberWithBooks.Books == null || memberWithBooks.Books.Count < 0)
			{
				_logger.LogWarning("No books found for member with IIN: {MemberIIN}", memberIIN);

				// Display warning message using MessageBox instead of throwing an error
				MessageBoxController.ShowWarning($"No books found for member with IIN: {memberIIN}.");

				// Optionally clear the DataGridView as well
				dataGridView.DataSource = null;

				return false;
			}

			_logger.LogInformation("Successfully retrieved {BookCount} books for member with IIN: {MemberIIN}", memberWithBooks.Books.Count, memberIIN);

			// Populate DataGridView with the member's books
			dataGridView.DataSource = memberWithBooks.Books
				.Select(book => new
				{
					memberWithBooks.IIN,
					BookID = book.Id,
					BookTitle = book.Title,
					// BookAuthor = book.Author // Include additional fields if available
				})
				.ToList();

			return true;
		}
		#endregion
	}
}
