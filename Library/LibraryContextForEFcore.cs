using Microsoft.EntityFrameworkCore;
using Library.Infrastructure.EFConfigurations;
using Library.Domain.Models;
namespace Library
{
    internal class LibraryContextForEFcore : DbContext
	{
		internal DbSet<Member> Members { get; set; } = null!;
		internal DbSet<Book> Books { get; set; } = null!;
		internal DbSet<Author> Authors { get; set; } = null!;
		private readonly string ConnectionString = Properties.Settings.Default.LibraryConnectionString;

		protected override void OnConfiguring(DbContextOptionsBuilder dbOptions)
		{
			dbOptions.UseSqlServer(ConnectionString);
			dbOptions.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new EntityConfigurationForAuthor());
			modelBuilder.ApplyConfiguration(new EntityConfigurationForMember());
			modelBuilder.ApplyConfiguration(new EntityConfigurationForBook());
		}
	}
}
