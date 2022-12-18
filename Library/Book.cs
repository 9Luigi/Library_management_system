using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    internal class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Genre { get; set; }
        //TODO createIndexForEachProperty and in Subscriber too (?)
        public string? Sort { get; set; } //new,classic,bestseller
        public DateTime PublicationDate { get; set; }
        public byte[] coverImage { get; set; }
        public int Amount { get; set; }
        public List<Member> Members { get; set; } = new();
        public List<Author> Authors { get; set; } = new();
        public byte[] BookVersion { get; set; }
        public Book() { }
        public Book(string title, string genre, DateTime publicationDate, int amount, string? description = "None",string? sort="None")
        {
            Title = title;
            Description = description;
            Genre = genre;
            Sort = sort;
            PublicationDate = publicationDate;
            Amount = amount;
        }
    }
}
