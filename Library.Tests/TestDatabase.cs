using Library.Server.Data;
using Library.Server.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Library.Tests;

/// <summary>
/// A separate PostgreSQL database for one test: created by the real migrations, dropped after Dispose.
/// Tests don't see each other's data and don't depend on the order they run in.
/// </summary>
internal sealed class TestDatabase : IDisposable
{
	private readonly DbContextOptions<LibraryContext> _options;

	public TestDatabase(PostgresFixture postgres)
	{
		var connectionString = new NpgsqlConnectionStringBuilder(postgres.ConnectionString)
		{
			Database = $"test_{Guid.NewGuid():N}"
		}.ConnectionString;

		_options = new DbContextOptionsBuilder<LibraryContext>()
			.UseNpgsql(connectionString)
			.Options;

		// Creates the database and applies the migrations, the same way the server does at startup
		using var context = CreateContext();
		context.Database.Migrate();
	}

	public LibraryContext CreateContext() => new(_options);

	public Member AddMember(string iin = "900101300123")
	{
		var member = new Member("Ivan", "Ivanov", new DateTime(1990, 1, 1), "Almaty, Abay 1", iin, "+7(777) 123-4567");
		using var context = CreateContext();
		context.Members.Add(member);
		context.SaveChanges();
		return member;
	}

	public void Dispose()
	{
		using var context = CreateContext();
		context.Database.EnsureDeleted();
	}
}
