using Library.Contracts.Members;
using Library.Server.Data;
using Library.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Server.Services;

/// <summary>
/// Members CRUD. The context is passed in by DI: one per HTTP request.
/// "Not found" is returned as null / false, the controller turns it into 404.
/// </summary>
public class MemberService(LibraryContext context, ILogger<MemberService> logger)
{
	public async Task<List<MemberDto>> GetAllAsync()
	{
		return await context.Members
			.AsNoTracking()
			.OrderBy(m => m.RegistrationDate)
			.Select(m => ToDto(m))
			.ToListAsync();
	}

	public async Task<MemberDto?> GetByIinAsync(string iin)
	{
		var member = await context.Members.AsNoTracking().FirstOrDefaultAsync(m => m.IIN == iin);
		return member == null ? null : ToDto(member);
	}

	public Task<bool> ExistsAsync(string iin) => context.Members.AnyAsync(m => m.IIN == iin);

	public async Task<MemberDto> CreateAsync(CreateMemberRequest request)
	{
		var member = new Member(request.Name, request.Surname, request.BirthDay, request.Address,
			request.IIN, request.PhoneNumber, request.Patronymic);

		context.Members.Add(member);
		await context.SaveChangesAsync();

		logger.LogInformation("Member with IIN {IIN} created", member.IIN);
		return ToDto(member);
	}

	/// <returns>The updated member, or null if there is no member with this IIN.</returns>
	public async Task<MemberDto?> UpdateAsync(string iin, UpdateMemberRequest request)
	{
		var member = await context.Members.FirstOrDefaultAsync(m => m.IIN == iin);
		if (member == null) return null;

		member.Name = request.Name;
		member.Surname = request.Surname;
		member.Patronymic = string.IsNullOrWhiteSpace(request.Patronymic) ? "None" : request.Patronymic;
		member.BirthDay = request.BirthDay;
		member.Age = Member.CalculateAge(request.BirthDay, DateTime.Today);
		member.Address = request.Address;
		member.PhoneNumber = request.PhoneNumber;

		// The context is tracking the member, so only the changed columns are saved
		await context.SaveChangesAsync();

		logger.LogInformation("Member with IIN {IIN} updated", iin);
		return ToDto(member);
	}

	/// <returns>False if there is no member with this IIN.</returns>
	public async Task<bool> DeleteAsync(string iin)
	{
		int deleted = await context.Members.Where(m => m.IIN == iin).ExecuteDeleteAsync();

		if (deleted > 0) logger.LogInformation("Member with IIN {IIN} deleted", iin);
		return deleted > 0;
	}

	private static MemberDto ToDto(Member m) => new(
		m.IIN, m.Name, m.Surname, m.Patronymic, m.BirthDay, m.Age, m.Address, m.PhoneNumber, m.RegistrationDate);
}
