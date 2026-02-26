using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Library.Infrastructure.Repositories;
using Library.Presentation.Controllers;
using System;

namespace Library.Application.Services.CRUD
{

	internal class MemberService
	{
		private readonly ILogger _logger;
		private readonly GenericRepository<Member> _memberRepository;

		internal MemberService(ILogger logger, GenericRepository<Member> memberRepository, LibraryContextForEFcore context)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
		}
		#region CRUD

		internal async Task<List<dynamic>> GetFilteredMembersAsync(LibraryContextForEFcore dbOcntext, long IIN)
		{
			try
			{
				var results = await dbOcntext.Set<Member>().Where(m => m.IIN == IIN)
					.Select(m => new
					{
						m.IIN,
						m.Name,
						m.Surname,
						m.Age,
						m.RegistrationDate,
						Books = m.Books.Select(b => b.Title).ToList()
					})
					.ToListAsync(
				);

				// Projection to dynamic
				return results.Select(member => (dynamic)new //TODO : refactor to avoid dynamic
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

		internal async Task<List<dynamic>> GetMembersAsync(LibraryContextForEFcore dbContext)
		{
			try
			{
				_logger.LogInformation("GetMembersAsync started.");

				// Retrieve members with their borrowed books
				var members = await dbContext.Set<Member>().Select(
					m => new
					{
						m.IIN,
						m.Name,
						m.Surname,
						m.Age,
						m.RegistrationDate,
						Books = string.Join(", ", m.Books.Select(b => b.Title)) // Join book titles
					}).ToListAsync();

				// Return the members as dynamic objects
				return members.Select(m => (dynamic)m).ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while retrieving members from the database.");
				throw;
			}
		}
		internal async Task<byte[]> GetMemberPhotoByIINAsync(LibraryContextForEFcore _dbContext, long IIN)
		{
			var photoBytes = await _dbContext.Members
				.AsNoTracking()
				.Where(m => m.IIN == IIN)
				.Select(m => m.Photo)
				.FirstOrDefaultAsync();
			if (photoBytes == null || photoBytes.Length == 0)
			{
				_logger.LogWarning("No photo found for member with IIN: {IIN}", IIN);
				throw new KeyNotFoundException($"No photo found for member with IIN: {IIN}");
			}
			return photoBytes;
		}
		public async Task<int> GetBorrowedBooksCountAsync(LibraryContextForEFcore dbContext, long IIN)
		{

			return await dbContext.Members
				.Where(m => m.IIN == IIN)
				.Select(m => m.Books.Count)  // Get the count of borrowed books
				.FirstOrDefaultAsync();
		}
		internal async Task<Member> GetMemberWithBorrowedBooksAsync(LibraryContextForEFcore dbContext,long IIN) //TODO : =>dynamic=>DTO
		{
			try
			{
				// Log entry into the method
				_logger.LogInformation("Attempting to retrieve member data and borrowed books for IIN: {IIN}", IIN);

				// Define the projection to retrieve member data along with their borrowed books
				var member = await dbContext.Set<Member>().Where(m => m.IIN == IIN)
					.Select(m => new Member
					{

						IIN = m.IIN,
						Books = m.Books.Select(b => new Book
						{
							Id = b.Id,
							Title = b.Title,
							// Add other necessary book fields here
						}).ToList()
					})
					.FirstOrDefaultAsync();
				

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

		internal async Task<bool> CreateMemberAsync(LibraryContextForEFcore dbContext ,Member memberToCreate)
		{
			try
			{
				bool isCreated = await _memberRepository.AddAsync(dbContext, memberToCreate);

				if (isCreated)
				{
					_logger.LogInformation("New member created successfully.");
				}

				return isCreated;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while creating a new member.");
				throw;
			}
		}

		internal async Task<bool> GetMemberWithHisBooksToDataGridViewAsync
			(LibraryContextForEFcore dbContext, long memberIIN, DataGridView dataGridView)
		{
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
		internal async Task<bool> UpdateMemberChangedFieldsAsync(LibraryContextForEFcore dbContext, Member memberToUpdate)
		{
			return await _memberRepository.UpdateAttachedFieldsAsync(dbContext!, memberToUpdate);
		}
		internal async Task<Member> GetMemberByIINForUpdateAsync(LibraryContextForEFcore dbContext, long IIN)
		{
			return await _memberRepository.GetByFieldAsync(dbContext!, m => m.IIN, IIN);
		}
		#endregion
		#region Validation
		internal async Task<bool> IsIINExistsAsync(LibraryContextForEFcore dbContext, long IIN)
		{
			bool isIINExists = await _memberRepository.AnyAsync(dbContext, m => m.IIN == IIN);
			if (isIINExists)
			{
				MessageBoxController.ShowWarning($"A member with IIN {IIN} already exists.");
				return false;
			}
			return true;
		}
		#endregion
	}
}
