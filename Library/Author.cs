using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    internal class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Biography { get; set; }
        public byte[] Photo { get; set; }
        public List<Book> Books { get; set; } = new();
        public Author() { }
        public Author(string name, string surname, string biography="None")
        {
            Name = name;
            Surname = surname;
            Biography = biography;
        }
    }
}
