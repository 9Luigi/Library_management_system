using Library.Data;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Library.Data.Repositories;
using System;

namespace Library.Services
{

	internal class MemberService
	{
		private readonly ILogger _logger;
		private readonly Repository<Member> _memberRepository;

		internal MemberService(ILogger logger, Repository<Member> memberRepository, LibraryContext context)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
		}
		#region CRUD

		internal async Task<List<dynamic>> GetFilteredMembersAsync(LibraryContext dbOcntext, long IIN)
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

		internal async Task<List<dynamic>> GetMembersAsync(LibraryContext dbContext)
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
		/// <summary>
		/// Returns the member's photo bytes, or null if the member has no photo.
		/// </summary>
		internal async Task<byte[]?> GetMemberPhotoAsync(LibraryContext dbContext, long IIN)
		{
			var photoPath = await dbContext.Members
				.AsNoTracking()
				.Where(m => m.IIN == IIN)
				.Select(m => m.PhotoPath)
				.FirstOrDefaultAsync();

			return LoadPhoto(photoPath);
		}

		/// <summary>
		/// Reads a photo file by the file name stored in Member.PhotoPath. Null if there is none.
		/// </summary>
		internal byte[]? LoadPhoto(string? photoPath)
		{
			var photo = ImageStorage.Load(ImageKind.Member, photoPath);
			if (photoPath != null && photo == null)
			{
				_logger.LogWarning("Photo file {PhotoPath} is missing in {Folder}", photoPath, ImageStorage.Folder(ImageKind.Member));
			}
			return photo;
		}
		public async Task<int> GetBorrowedBooksCountAsync(LibraryContext dbContext, long IIN)
		{

			return await dbContext.Members
				.Where(m => m.IIN == IIN)
				.Select(m => m.Books.Count)  // Get the count of borrowed books
				.FirstOrDefaultAsync();
		}
		internal async Task<Member> GetMemberWithBorrowedBooksAsync(LibraryContext dbContext,long IIN) //TODO : =>dynamic=>DTO
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

		/// <summary>
		/// Creates the member, then saves the photo file named by the new Id and stores its file name.
		/// </summary>
		internal async Task<bool> CreateMemberAsync(LibraryContext dbContext, Member memberToCreate, byte[]? photo)
		{
			try
			{
				bool isCreated = await _memberRepository.AddAsync(dbContext, memberToCreate);

				if (isCreated)
				{
					_logger.LogInformation("New member created successfully.");

					// The file is named by Id, which exists only after the first save
					if (photo is { Length: > 0 })
					{
						memberToCreate.PhotoPath = ImageStorage.Save(ImageKind.Member, memberToCreate.Id, photo);
						await dbContext.SaveChangesAsync();
					}
				}

				return isCreated;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred while creating a new member.");
				throw;
			}
		}

		/// <summary>
		/// Returns the books currently borrowed by the member. Empty list if the member is missing or has no books.
		/// </summary>
		internal async Task<List<MemberBookRow>> GetMemberBooksAsync(LibraryContext dbContext, long memberIIN)
		{
			_logger.LogInformation("Start fetching books for member with IIN: {MemberIIN}", memberIIN);

			var rows = await dbContext.Members
				.Where(m => m.IIN == memberIIN)
				.SelectMany(m => m.Books.Select(b => new MemberBookRow(m.IIN, b.Id, b.Title)))
				.ToListAsync();

			_logger.LogInformation("Retrieved {BookCount} books for member with IIN: {MemberIIN}", rows.Count, memberIIN);
			return rows;
		}
		/// <summary>
		/// Saves changed fields and, if a new photo was chosen, replaces the photo file.
		/// </summary>
		/// <returns>True if anything changed (fields or photo).</returns>
		internal async Task<bool> UpdateMemberChangedFieldsAsync(LibraryContext dbContext, Member memberToUpdate, byte[]? newPhoto)
		{
			bool photoChanged = false;
			if (newPhoto is { Length: > 0 })
			{
				// Usually the same file name ("15.png"), so the database may not change while the file does
				memberToUpdate.PhotoPath = ImageStorage.Save(ImageKind.Member, memberToUpdate.Id, newPhoto);
				photoChanged = true;
			}

			bool fieldsChanged = await _memberRepository.UpdateAttachedFieldsAsync(dbContext!, memberToUpdate);
			return fieldsChanged || photoChanged;
		}

		/// <summary>
		/// Deletes the member and the member's photo file.
		/// </summary>
		internal async Task<bool> DeleteMemberAsync(LibraryContext dbContext, long IIN)
		{
			int memberId = await dbContext.Members
				.Where(m => m.IIN == IIN)
				.Select(m => m.Id)
				.FirstOrDefaultAsync();

			bool isDeleted = await _memberRepository.DeleteAsync(dbContext, m => m.IIN, IIN);
			if (isDeleted)
			{
				ImageStorage.Delete(ImageKind.Member, memberId);
				_logger.LogInformation("Member with IIN {IIN} and the photo were deleted.", IIN);
			}
			return isDeleted;
		}
		internal async Task<Member> GetMemberByIINForUpdateAsync(LibraryContext dbContext, long IIN)
		{
			return await _memberRepository.GetByFieldAsync(dbContext!, m => m.IIN, IIN);
		}
		#endregion
		#region Validation
		/// <summary>
		/// Returns true if a member with this IIN already exists.
		/// </summary>
		internal async Task<bool> IsIINExistsAsync(LibraryContext dbContext, long IIN)
		{
			return await _memberRepository.AnyAsync(dbContext, m => m.IIN == IIN);
		}
		#endregion
	}
}
