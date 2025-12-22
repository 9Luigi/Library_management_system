using System.Text.RegularExpressions;

namespace Library.Presentation.Controllers
{
	static internal class RegexController
	{
		#region Regex constants
		const string nameAndPatronomycPattern = @"^[A-Za-zА-Яа-яЁё][A-Za-zА-Яа-яЁё' -]*[A-Za-zА-Яа-яЁё]?$";
		const string birthdayPattern = @"^(0[1-9]|[12][0-9]|3[01])[-./](0[1-9]|1[0-2])[-./](19|20)\d{2}$";
		const string surnamePattern = @"^[A-Za-zА-Яа-яЁё][A-Za-zА-Яа-яЁё' .-]*[A-Za-zА-Яа-яЁё]?$";
		const string phoneNumberPattern = @"^\+7\(\d{3}\)\s\d{3}-\d{4}$";
		const string AdressPattern = @"^[A-Za-zА-Яа-яЁё\s.,'-]+(?:\s+[0-9A-Za-zА-Яа-яЁё/\\-]+)*$";
		#endregion

		static string? selectedPattern;
		static string? correctSymbols;

		#region Correct messages
		const string correctNameAndPatronymicSymbols = "latin, cyrillic characters and sign -'";
		const string correctSurnameSymbols = "latin, cyrillic characters and signs -.'";
		static readonly DateTime minimumAge = DateTime.Now.AddYears(-14);
		static readonly string correctBirdthday = $"from 01.01.1900 to {minimumAge:dd.MM.yyyy}";

		const string correctAdress = "Examples: 'New York, 5th Ave', 'Moscow, Arbat 25', 'London NW1 5TR'";
		const string correctPhoneNumber = "Format: +7(XXX) XXX-XXXX";
		#endregion

		static public bool Check(string input, Control control)
		{
			selectedPattern = null;
			correctSymbols = null;

			switch (control.Name)
			{
				case "TBName":
				case "TBPatronymic":
					selectedPattern = nameAndPatronomycPattern;
					correctSymbols = correctNameAndPatronymicSymbols;
					break;
				case "MTBBirthday":
					selectedPattern = birthdayPattern;
					correctSymbols = correctBirdthday;
					break;
				case "TBSurname":
					selectedPattern = surnamePattern;
					correctSymbols = correctSurnameSymbols;
					break;
				case "MTBPhoneNumber":
					selectedPattern = phoneNumberPattern;
					correctSymbols = correctPhoneNumber;
					break;
				case "MTBAdress":
					selectedPattern = AdressPattern;
					correctSymbols = correctAdress;
					break;
				default:
					MessageBoxController.ShowWarning($"RegexController.Check: no pattern for control {control.Name}");
					return false;
			}

			if (string.IsNullOrEmpty(input))
			{
				MessageBoxController.ShowWarning($"Field {control.Name} cannot be empty");
				return false;
			}

			if (selectedPattern != null)
			{
				Regex regex = new(selectedPattern, RegexOptions.IgnoreCase);
				if (!regex.IsMatch(input))
				{
					MessageBoxController.ShowWarning($"Value of field {control.Name} is not correct.\n" +
						$"Correct format: {correctSymbols}");
					return false;
				}

				if (control.Name == "MTBBirthday" && !ValidateBirthDate(input))
				{
					MessageBoxController.ShowWarning($"Birth date is not valid. {correctBirdthday}");
					return false;
				}

				return true;
			}

			return false;
		}

		static private bool ValidateBirthDate(string dateString)
		{
			string normalizedDate = dateString.Replace('/', '.').Replace('-', '.');

			if (DateTime.TryParseExact(normalizedDate, "dd.MM.yyyy",
				System.Globalization.CultureInfo.InvariantCulture,
				System.Globalization.DateTimeStyles.None, out DateTime birthDate))
			{
				DateTime minDate = new DateTime(1900, 1, 1);
				DateTime maxDate = minimumAge;

				return birthDate >= minDate && birthDate <= maxDate;
			}

			return false;
		}
	}
}