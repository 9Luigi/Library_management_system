using System.Text.RegularExpressions;

namespace Library.Controllers
{
    static internal class RegexController
    {
		#region Regex constants
		const string nameAndPatronomycPattern = "^[a-zA-Zа-яА-ЯёЁ'-]+$";
		const string birthdayPattern = @"^(0?[1-9]|[1-2][0-9]|3[01])(\/|.)(0?[1-9]|1[0-2])(\/|.)((19|20)\d\d)$"; 
        const string surnamePattern = "^[a-z,-.']+$|^[а-яё,.'-]+$";
        const string phoneNumberPattern = @"(\+7)(\([0-9]{3}\))\s([0-9]{3})-([0-9]{4})";
        const string AdressPattern = @"^([a-zA-Z]+|[а-яА-яё]+)\s([0-9]{1,3})\-([0-9]{1,3})$";
		#endregion
		static string? selectedPattern;
        static string? correctSymbols;
		#region Correct
		const string correctNameAndPatronymicSymbols = "latin, cyrillic characters and sign -";
        const string correctSurnameSymbols = "latin, cyrillic characters and signs -.'";
        static readonly DateTime minimumAge = DateTime.Now.AddYears(-14); 
        static readonly string correctBirdthday = $"from 01.01.1900 to {minimumAge}";
        const string correctAdress = "Example 168-32";
		#endregion
		static public bool Check(string input, Control control)
        {
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
                    break;
                case "MTBAdress":
                    selectedPattern = AdressPattern;
                    correctSymbols = correctAdress;
                    break;
                default:MessageBox.Show("RegexController.Check default invoked, not allowed!");
                    break;
            }
            if (selectedPattern != null)
            {
                Regex regex = new(selectedPattern, RegexOptions.IgnoreCase);
                if (!regex.IsMatch(input))
                {
                    MessageBox.Show($"value of field {control.Name} is not correct, " +
                        $"correct {control.Name} field may include {correctSymbols}");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else { return false; }
        }
    }
}
