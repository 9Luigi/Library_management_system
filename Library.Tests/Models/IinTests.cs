using Library.Server.Models;

namespace Library.Tests.Models;

public class IinTests
{
	[Fact]
	public void Create_EncodesBirthdayAndGender()
	{
		var iin = Iin.Create(new DateTime(1990, 5, 15), female: true, serial: 1234);

		Assert.NotNull(iin);
		Assert.StartsWith("900515" + "4" + "1234", iin); // 4 = female, born in the 1900s
	}

	[Fact]
	public void Create_BornIn2000s_UsesCenturyDigit5Or6()
	{
		Assert.Equal('5', Iin.Create(new DateTime(2005, 1, 1), female: false, serial: 1)?[6]);
	}

	[Fact]
	public void IsValid_ChangedDigit_IsFalse()
	{
		// Some serial numbers give no valid check digit, so take the first one that does
		string iin = Enumerable.Range(1000, 10)
			.Select(serial => Iin.Create(new DateTime(1990, 5, 15), false, serial))
			.First(i => i != null)!;
		// The 11th digit is not used: its weight in the first pass is 11, so changing it doesn't change the sum mod 11
		string broken = iin[..8] + (char)('0' + (iin[8] - '0' + 1) % 10) + iin[9..];

		Assert.True(Iin.IsValid(iin));
		Assert.False(Iin.IsValid(broken));
	}
}
