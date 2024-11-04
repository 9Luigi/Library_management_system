using Library.Models;
using Microsoft.EntityFrameworkCore;
using Library.EFConfigurations;
namespace Library
{
    internal class LibraryContextForEFcore : DbContext
	{
		internal DbSet<Member> Members { get; set; } = null!;
		internal DbSet<Book> Books { get; set; } = null!;
		internal DbSet<Author> Authors { get; set; } = null!;
		string ConnectionString = @"Server=" + 
			Environment.MachineName + 
			";DataBase=Library;User Id=Adm_RKudrik;Password=g7DcA)RH^qZw;MultipleActiveResultSets=true;Encrypt=False";
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
