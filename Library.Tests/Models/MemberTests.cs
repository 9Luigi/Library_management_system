using Library.Server.Models;

namespace Library.Tests.Models;

public class MemberTests
{
	private static readonly DateTime Today = new(2026, 6, 15);

	[Fact]
	public void CalculateAge_BirthdayAlreadyPassedThisYear_CountsFullYears()
	{
		Assert.Equal(30, Member.CalculateAge(new DateTime(1996, 6, 14), Today));
	}

	[Fact]
	public void CalculateAge_BirthdayIsToday_CountsFullYears()
	{
		Assert.Equal(30, Member.CalculateAge(new DateTime(1996, 6, 15), Today));
	}

	[Fact]
	public void CalculateAge_BirthdayIsTomorrow_IsOneYearLess()
	{
		Assert.Equal(29, Member.CalculateAge(new DateTime(1996, 6, 16), Today));
	}

	[Fact]
	public void Patronymic_NotGiven_IsNone()
	{
		var member = new Member("Ivan", "Ivanov", new DateTime(1990, 1, 1), "Almaty, Abay 1", 900101300123, "+7(777) 123-4567");

		Assert.Equal("None", member.Patronymic);
	}
}
