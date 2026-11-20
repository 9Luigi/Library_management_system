namespace Library.Contracts.Members;

/// <summary>
/// A member as the server returns it. IIN goes first: the client grid reads it from column 0.
/// IIN is a string: it can start with 0 (people born in 2000–2009), a number would lose that zero.
/// </summary>
public record MemberDto(
	string IIN,
	string Name,
	string Surname,
	string Patronymic,
	DateTime BirthDay,
	int Age,
	string Address,
	string PhoneNumber,
	DateTime RegistrationDate);
