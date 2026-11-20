using Library.Server.Models;

namespace Library.Server.Data;

/// <summary>
/// Fills an empty database with fictional members for local development (called only in Development).
/// The random generator has a fixed seed, so every run produces the same people.
/// </summary>
public static class DevDataSeeder
{
	private static readonly string[] MaleNames =
		["Aidar", "Arman", "Askar", "Azamat", "Baurzhan", "Daniyar", "Dauren", "Erlan", "Kairat", "Marat", "Nurlan", "Ruslan", "Serik", "Timur", "Yerzhan"];

	private static readonly string[] FemaleNames =
		["Aigerim", "Ainur", "Aliya", "Aruzhan", "Asel", "Botagoz", "Dana", "Dinara", "Gulnara", "Indira", "Kamila", "Madina", "Saule", "Togzhan", "Zhanna"];

	// Male form; the female form adds "a" (Akhmetov -> Akhmetova)
	private static readonly string[] Surnames =
		["Abenov", "Akhmetov", "Baimukhanov", "Bekov", "Dzhaksybekov", "Ermekov", "Iskakov", "Kasymov", "Mukanov", "Nurpeisov", "Omarov", "Sadykov", "Tulegenov", "Zhunusov"];

	private static readonly string[] FatherNames =
		["Asylbek", "Bolat", "Erlan", "Kairat", "Marat", "Nurlan", "Serik", "Talgat"];

	private static readonly string[] Cities = ["Almaty", "Astana", "Shymkent", "Karaganda", "Aktobe", "Pavlodar"];

	private static readonly string[] Streets = ["Abay", "Dostyk", "Tole Bi", "Satpayev", "Kabanbay Batyr", "Al-Farabi", "Respublika", "Seifullin"];

	private static readonly string[] MobileCodes = ["701", "702", "705", "707", "747", "771", "775", "777", "778"];

	public static void Seed(LibraryContext context, int count = 100)
	{
		if (context.Members.Any()) return;

		var random = new Random(42);
		int serial = 1000;

		for (int i = 0; i < count; i++)
		{
			bool female = random.Next(2) == 0;

			// 1950–1999: IINs of people born in 2000–2009 start with 0, and IIN is stored as a number,
			// so the leading zero would be lost
			var birthDay = new DateTime(random.Next(1950, 2000), 1, 1).AddDays(random.Next(365));

			string? iin;
			do iin = Iin.Create(birthDay, female, serial++);
			while (iin == null);

			string surname = Pick(random, Surnames) + (female ? "a" : "");
			string patronymic = Pick(random, FatherNames) + (female ? "ovna" : "ovich");
			string address = $"{Pick(random, Cities)}, {Pick(random, Streets)} {random.Next(1, 200)}";
			string phone = $"+7({Pick(random, MobileCodes)}) {random.Next(100, 1000)}-{random.Next(0, 10000):D4}";

			var member = new Member(Pick(random, female ? FemaleNames : MaleNames), surname, birthDay,
				address, long.Parse(iin), phone, patronymic)
			{
				// Spread over the last three years, so sorting by registration date makes sense
				RegistrationDate = DateTime.Now.AddDays(-random.Next(3 * 365)).AddMinutes(-random.Next(24 * 60))
			};
			context.Members.Add(member);
		}

		context.SaveChanges();
	}

	private static string Pick(Random random, string[] items) => items[random.Next(items.Length)];
}
