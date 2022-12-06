using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library
{
    internal class RegexController //TODO make static
    {
        const string nameAndPatronomycPattern = "^[a-z,-]+$|^[а-яё,-]+$";
        const string birthdayPattern = "(0?[1-9]|[1-2][0-9]|3[01])/(0?[1-9]|1[0-2])/((19|20)\\d\\d)";
        const string surnamePattern = "^[a-z,-.']+$|^[а-яё,.'-]+$";
        const string phoneNumberPattern = "(\\+7)([0-9]{10})";
        string? selectedPattern;
        public void Check(string input,Control control)//string str, TextBox textBox
        {
            switch (control.Name)
            {
                case "tbName":
                case "tbPatronymic":
                    selectedPattern = nameAndPatronomycPattern;
                    break;
                case "mtbBirthday":
                    selectedPattern = birthdayPattern;
                    break;
                case "tbSurname":
                    selectedPattern = surnamePattern;
                    break;
                case "mtbPhoneNumber":
                    selectedPattern = phoneNumberPattern;
                    break;
                default:
                    break;
            }
            if (selectedPattern != null)
            {
                Regex regex = new Regex(selectedPattern);
                if (!regex.IsMatch(input))
                {
                    MessageBox.Show($"value of field {control.Name} is wrong");
                }
                else
                {
                    MessageBox.Show($"value of field {control.Name} is well");
                }
            }
        }
    }
}
