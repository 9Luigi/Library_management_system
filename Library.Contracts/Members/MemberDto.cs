namespace Library.Contracts.Members;

/// <summary>
/// A member as the server returns it. IIN goes first: the client grid reads it from column 0.
/// </summary>
public record MemberDto(
	long IIN,
	string Name,
	string Surname,
	string Patronymic,
	DateTime BirthDay,
	int Age,
	string Address,
	string PhoneNumber,
	DateTime RegistrationDate);
