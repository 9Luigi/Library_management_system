using Library.Services;
using Microsoft.EntityFrameworkCore;

namespace Library.Tests.Services;

public class CirculationServiceTests : IDisposable
{
	private readonly TestDatabase _db = new();
	private readonly CirculationService _service;

	public CirculationServiceTests()
	{
		_service = new CirculationService(_db.CreateContext);
	}

	public void Dispose() => _db.Dispose();

	#region BorrowBook

	[Fact]
	public async Task BorrowBook_DecreasesAmountAndLinksBookToMember()
	{
		var book = _db.AddBook(amount: 3);
		var member = _db.AddMember();

		bool result = await _service.BorrowBook(book.Id, member.IIN);

		Assert.True(result);
		using var context = _db.CreateContext();
		var savedBook = await context.Books.Include(b => b.Members).SingleAsync(b => b.Id == book.Id);
		Assert.Equal(2, savedBook.Amount);
		Assert.Contains(savedBook.Members, m => m.IIN == member.IIN);
	}

	[Fact]
	public async Task BorrowBook_NoCopiesLeft_Throws()
	{
		var book = _db.AddBook(amount: 0);
		var member = _db.AddMember();

		await Assert.ThrowsAsync<InvalidOperationException>(() => _service.BorrowBook(book.Id, member.IIN));
	}

	[Fact]
	public async Task BorrowBook_SameBookTwice_Throws()
	{
		var book = _db.AddBook(amount: 5);
		var member = _db.AddMember();
		await _service.BorrowBook(book.Id, member.IIN);

		await Assert.ThrowsAsync<InvalidOperationException>(() => _service.BorrowBook(book.Id, member.IIN));
	}

	[Fact]
	public async Task BorrowBook_UnknownBook_Throws()
	{
		var member = _db.AddMember();

		await Assert.ThrowsAsync<InvalidOperationException>(() => _service.BorrowBook(bookID: 999, member.IIN));
	}

	[Fact]
	public async Task BorrowBook_UnknownMember_Throws()
	{
		var book = _db.AddBook();

		await Assert.ThrowsAsync<InvalidOperationException>(() => _service.BorrowBook(book.Id, IIN: 111111111111));
	}

	#endregion

	#region ReturnBook

	[Fact]
	public async Task ReturnBook_IncreasesAmountAndUnlinksBook()
	{
		var book = _db.AddBook(amount: 1);
		var member = _db.AddMember();
		await _service.BorrowBook(book.Id, member.IIN);

		bool result = await _service.ReturnBook(book.Id, member.IIN);

		Assert.True(result);
		using var context = _db.CreateContext();
		var savedBook = await context.Books.Include(b => b.Members).SingleAsync(b => b.Id == book.Id);
		Assert.Equal(1, savedBook.Amount);
		Assert.Empty(savedBook.Members);
	}

	[Fact]
	public async Task ReturnBook_NotBorrowedByMember_Throws()
	{
		var book = _db.AddBook();
		var member = _db.AddMember();

		await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ReturnBook(book.Id, member.IIN));
	}

	#endregion
}
