namespace Library.Server.Models;

public class Member
{
	public int Id { get; set; }
	public string Name { get; set; } = "";
	public string Surname { get; set; } = "";
	public string Patronymic { get; set; } = "None";
	public int Age { get; set; }
	public DateTime BirthDay { get; set; }
	public string Address { get; set; } = "";
	public string PhoneNumber { get; set; } = "";
	public string IIN { get; set; } = "";
	public DateTime RegistrationDate { get; set; }

	public Member() { }

	public Member(string name, string surname, DateTime birthday, string address, string iin, string phoneNumber, string? patronymic = null)
	{
		Name = name;
		Surname = surname;
		Patronymic = string.IsNullOrWhiteSpace(patronymic) ? "None" : patronymic;
		RegistrationDate = DateTime.Now;
		BirthDay = birthday;
		Address = address;
		PhoneNumber = phoneNumber;
		IIN = iin;
		Age = CalculateAge(BirthDay, RegistrationDate);
	}

	/// <summary>
	/// Full years between the birthday and the given date (one less if the birthday hasn't come yet that year).
	/// </summary>
	public static int CalculateAge(DateTime birthDay, DateTime today)
	{
		int age = today.Year - birthDay.Year;
		if (birthDay > today.AddYears(-age)) age--;
		return age;
	}
}
