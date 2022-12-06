using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library
{
    internal class RegexController
    {
        public void check()//string str, TextBox textBox
        {
            string nameCheckExpression = "^[a-z,-]+$|^[а-яё,-]+$";
            string birthdayCheckExpression = "(0?[1-9]|[1-2][0-9]|3[01])/(0?[1-9]|1[0-2])/((19|20)\\d\\d)";
            string surnameCheckRegex = "^[a-z,-.']+$|^[а-яё,.'-]+$";
            string phoneNumberCheck = "(\\+7)([0-9]{10})";
            Regex regex = new Regex(phoneNumberCheck, RegexOptions.IgnoreCase);
            MessageBox.Show(regex.Match("+77142572004").ToString()); 
            MessageBox.Show(regex.Match("+77762475879").ToString());
        }
    }
}
