using Library.Models;

namespace Library.Tests.Models;

public class MemberTests
{
	private static Member CreateWithBirthday(DateTime birthday) =>
		new("Ivan", "Ivanov", birthday, "Almaty, Abay 1", 900101300123, "+7(777) 123-4567");

	[Fact]
	public void Age_BirthdayAlreadyPassedThisYear_CountsFullYears()
	{
		var member = CreateWithBirthday(DateTime.Today.AddYears(-30).AddDays(-1));

		Assert.Equal(30, member.Age);
	}

	[Fact]
	public void Age_BirthdayIsTomorrow_IsOneYearLess()
	{
		var member = CreateWithBirthday(DateTime.Today.AddYears(-30).AddDays(1));

		Assert.Equal(29, member.Age);
	}

	[Fact]
	public void Patronymic_NotGiven_IsNone()
	{
		var member = CreateWithBirthday(new DateTime(1990, 1, 1));

		Assert.Equal("None", member.Patronymic);
	}
}
