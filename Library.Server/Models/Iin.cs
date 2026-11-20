namespace Library.Server.Models;

/// <summary>
/// Kazakhstan IIN (12 digits): YYMMDD + century/gender digit + 4-digit serial number + check digit.
/// </summary>
public static class Iin
{
	private static readonly int[] FirstWeights = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
	private static readonly int[] SecondWeights = [3, 4, 5, 6, 7, 8, 9, 10, 11, 1, 2];

	/// <summary>
	/// Builds an IIN, or returns null if this serial number gives no valid check digit (such IINs are not issued).
	/// </summary>
	public static string? Create(DateTime birthDay, bool female, int serial)
	{
		// 7th digit: 1/2 born in the 1800s, 3/4 in the 1900s, 5/6 in the 2000s (odd = male, even = female)
		int centuryDigit = (birthDay.Year / 100 - 18) * 2 + 1 + (female ? 1 : 0);
		string first11 = $"{birthDay:yyMMdd}{centuryDigit}{serial:D4}";

		int? check = CheckDigit(first11);
		return check == null ? null : first11 + check;
	}

	/// <summary>
	/// True if the IIN has 12 digits and a correct check digit.
	/// </summary>
	public static bool IsValid(string iin) =>
		iin.Length == 12 && iin.All(char.IsDigit) && CheckDigit(iin[..11]) == iin[11] - '0';

	private static int? CheckDigit(string first11)
	{
		int check = WeightedSum(first11, FirstWeights) % 11;
		if (check == 10) check = WeightedSum(first11, SecondWeights) % 11;
		return check == 10 ? null : check;
	}

	private static int WeightedSum(string digits, int[] weights) =>
		digits.Select((c, i) => (c - '0') * weights[i]).Sum();
}
