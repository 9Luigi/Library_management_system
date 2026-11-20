using Library.Server.Data.Configurations;
using Library.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Server.Data;

/// <summary>
/// Options (provider, connection string) come from Program.cs via DI.
/// </summary>
public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
{
	public DbSet<Member> Members => Set<Member>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new MemberConfiguration());
	}
}
