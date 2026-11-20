using System.ComponentModel.DataAnnotations;

namespace Library.Contracts.Members;

/// <summary>
/// Body of PUT api/members/{iin}. IIN is taken from the URL and cannot be changed.
/// </summary>
public record UpdateMemberRequest(
	[Required, StringLength(75)] string Name,
	[Required, StringLength(75)] string Surname,
	[StringLength(75)] string? Patronymic,
	DateTime BirthDay,
	[Required] string Address,
	[Required, StringLength(75)] string PhoneNumber);
