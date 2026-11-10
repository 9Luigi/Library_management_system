using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Server.Models
{
    internal class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Biography { get; set; }
        /// <summary>Photo file name inside the authors images folder (e.g. "3.png"), null if there is no photo.</summary>
        public string? PhotoPath { get; set; }
        public List<Book> Books { get; set; } = new();
        public Author() { }
        public Author(string name, string surname, string biography = "None")
        {
            Name = name;
            Surname = surname;
            Biography = biography;
        }
    }
}

