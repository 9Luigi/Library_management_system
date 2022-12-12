using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    internal class LibraryContextForEFcore : DbContext
    {
        internal DbSet<Member> Members { get; set; } = null!;
        internal DbSet<Book> Books { get; set; } = null!;
        internal DbSet<Author> Authors { get; set; } = null!;
        string ConnectionString = @"Server=" + Environment.MachineName + ";DataBase=LibraryDB;User Id=RomanKudrik;Password=98585R;MultipleActiveResultSets=true;Encrypt=False";
        protected  override void OnConfiguring(DbContextOptionsBuilder dbOptions)
        {
            dbOptions.UseSqlServer(ConnectionString);
            //dbOptions.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EntityConfigurationForAuthor());
            modelBuilder.ApplyConfiguration(new EntityConfigurationForMember());
            modelBuilder.ApplyConfiguration(new EntityConfigurationForBook());
        }
    }
}
