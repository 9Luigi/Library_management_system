using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library
{
    static internal class RegexController
    {
        const string nameAndPatronomycPattern = "^[a-z,-]+$|^[а-яё,-]+$";//TODO 1 symbol in name?
        const string birthdayPattern = @"^(0?[1-9]|[1-2][0-9]|3[01])(\/|.)(0?[1-9]|1[0-2])(\/|.)((19|20)\d\d)$"; //TODO last year should start with currentYear-14 (?)
        const string surnamePattern = "^[a-z,-.']+$|^[а-яё,.'-]+$";
        const string phoneNumberPattern = @"(\+7)(\([0-9]{3}\))\s([0-9]{3})-([0-9]{4})";
        const string AdressPattern = @"^([a-zA-Z]+|[а-яА-яё]+)\s([0-9]{1,3})\-([0-9]{1,3})$";
        static string? selectedPattern;
        static string? correctSymbols;
        static string correctNameAndPatronymicSymbols = "latin, cyrillic characters and sign -";
        static string correctSurnameSymbols = "latin, cyrillic characters and signs -.'";
        static DateTime minimumAge = DateTime.Now.AddYears(-14); //
        static string correctBirdthday = $"from 01.01.1900 to {minimumAge.ToString()}";
        static string correctAdress = "Example 168-32";
        static public bool Check(string input,Control control)
        {
            switch (control.Name)
            {
                case "tbName":
                case "tbPatronymic":
                    selectedPattern = nameAndPatronomycPattern;
                    correctSymbols = correctNameAndPatronymicSymbols;
                    break;
                case "mtbBirthday":
                    selectedPattern = birthdayPattern;
                    correctSymbols = correctBirdthday;
                    break;
                case "tbSurname":
                    selectedPattern = surnamePattern;
                    correctSymbols = correctSurnameSymbols;
                    break;
                case "mtbPhoneNumber":
                    selectedPattern = phoneNumberPattern;
                    break;
                case "mtbAdress":
                    selectedPattern = AdressPattern;
                    correctSymbols = correctAdress;
                    break;
                default:
                    break;
            }
            if (selectedPattern != null)
            {
                Regex regex = new Regex(selectedPattern,RegexOptions.IgnoreCase);
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
