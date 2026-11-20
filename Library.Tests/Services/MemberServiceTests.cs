using Library.Data.Repositories;
using Library.Models;
using Library.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace Library.Tests.Services;

public class MemberServiceTests : IDisposable
{
	private readonly TestDatabase _db = new();
	private readonly MemberService _service;

	public MemberServiceTests()
	{
		// NullLogger: the tests don't need log files
		_service = new MemberService(NullLogger.Instance, new Repository<Member>(), _db.CreateContext());
	}

	public void Dispose() => _db.Dispose();

	[Fact]
	public async Task IsIINExistsAsync_ReturnsTrueOnlyForExistingMember()
	{
		var member = _db.AddMember(iin: 900101300123);
		using var context = _db.CreateContext();

		Assert.True(await _service.IsIINExistsAsync(context, member.IIN));
		Assert.False(await _service.IsIINExistsAsync(context, 111111111111));
	}

	[Fact]
	public async Task GetMemberBooksAsync_ReturnsBorrowedBooks()
	{
		var member = _db.AddMember();
		var book1 = _db.AddBook("First");
		var book2 = _db.AddBook("Second");
		var circulation = new CirculationService(_db.CreateContext);
		await circulation.BorrowBook(book1.Id, member.IIN);
		await circulation.BorrowBook(book2.Id, member.IIN);

		using var context = _db.CreateContext();
		var rows = await _service.GetMemberBooksAsync(context, member.IIN);

		Assert.Equal(2, rows.Count);
		Assert.All(rows, r => Assert.Equal(member.IIN, r.IIN));
		Assert.Equal(["First", "Second"], rows.Select(r => r.BookTitle).Order());
	}

	[Fact]
	public async Task GetMemberBooksAsync_UnknownMember_ReturnsEmptyList()
	{
		using var context = _db.CreateContext();

		var rows = await _service.GetMemberBooksAsync(context, 111111111111);

		Assert.Empty(rows);
	}

	[Fact]
	public async Task GetMembersPageAsync_ReturnsRequestedPageAndTotalCount()
	{
		for (int i = 0; i < 5; i++)
		{
			_db.AddMember(iin: 900101300000 + i);
		}
		using var context = _db.CreateContext();

		var (members, totalCount) = await _service.GetMembersPageAsync(context, page: 2, pageSize: 2);

		Assert.Equal(5, totalCount);
		Assert.Equal(2, members.Count);
	}
}
