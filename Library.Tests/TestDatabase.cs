using Library.Data;
using Library.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Library.Tests;

/// <summary>
/// Temporary SQLite database in memory: one per test, gone after Dispose.
/// The in-memory database lives as long as its connection is open, so the connection is kept here
/// and every context created by this class shares it (and therefore sees the same data).
/// </summary>
internal sealed class TestDatabase : IDisposable
{
	private readonly SqliteConnection _connection;
	private readonly DbContextOptions<LibraryContext> _options;

	public TestDatabase()
	{
		_connection = new SqliteConnection("DataSource=:memory:");
		_connection.Open();

		_options = new DbContextOptionsBuilder<LibraryContext>()
			.UseSqlite(_connection)
			.Options;

		// Builds the tables from the model (no migrations needed for tests)
		using var context = CreateContext();
		context.Database.EnsureCreated();
	}

	public LibraryContext CreateContext() => new(_options);

	public Book AddBook(string title = "Test book", int amount = 1)
	{
		var book = new Book(title, "Novel", new DateTime(2000, 1, 1), amount);
		using var context = CreateContext();
		context.Books.Add(book);
		context.SaveChanges();
		return book;
	}

	public Member AddMember(long iin = 900101300123)
	{
		var member = new Member("Ivan", "Ivanov", new DateTime(1990, 1, 1), "Almaty, Abay 1", iin, "+7(777) 123-4567");
		using var context = CreateContext();
		context.Members.Add(member);
		context.SaveChanges();
		return member;
	}

	public void Dispose() => _connection.Dispose();
}
