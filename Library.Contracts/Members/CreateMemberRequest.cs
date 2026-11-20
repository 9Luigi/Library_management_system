using System.ComponentModel.DataAnnotations;

namespace Library.Contracts.Members;

/// <summary>
/// Body of POST api/members. Age and registration date are set by the server.
/// </summary>
public record CreateMemberRequest(
	[Range(100000000000, 999999999999, ErrorMessage = "IIN must be 12 digits.")] long IIN,
	[Required, StringLength(75)] string Name,
	[Required, StringLength(75)] string Surname,
	[StringLength(75)] string? Patronymic,
	DateTime BirthDay,
	[Required] string Address,
	[Required, StringLength(75)] string PhoneNumber);
