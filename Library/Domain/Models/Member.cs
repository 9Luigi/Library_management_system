using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library.Domain.Models
{
    internal class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int Age { get; set; }
        public DateTime BirthDay { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public long IIN { get; set; }
        public DateTime RegistrationDate { get; set; }
        public byte[]? Photo { get; set; }
        [Timestamp]
        public byte[] MemberVersion { get; set; }
        public List<Book> Books { get; set; } = new();
        public Member() { }

        public Member(string name, string surname, DateTime birthday, string adress, long iin, string Phonenumber, byte[] photo, string patronymic = "None")
        {
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            RegistrationDate = DateTime.Now;
            BirthDay = birthday;
            Adress = adress;
            PhoneNumber = Phonenumber;
            IIN = iin;
            Photo = photo;
            Age = RegistrationDate.Year - BirthDay.Year;
            if (BirthDay > RegistrationDate.AddYears(-Age)) Age--;
            if (Patronymic == null)
            {
                Patronymic = "None";
            }
        }
    }
}
