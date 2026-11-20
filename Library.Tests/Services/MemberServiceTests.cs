using Library.Contracts.Members;
using Library.Server.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace Library.Tests.Services;

[Collection("Postgres")]
public class MemberServiceTests(PostgresFixture postgres) : IDisposable
{
	private readonly TestDatabase _db = new(postgres);

	public void Dispose() => _db.Dispose();

	// A new service with a new context, as on the server: one per HTTP request
	private MemberService CreateService() => new(_db.CreateContext(), NullLogger<MemberService>.Instance);

	[Fact]
	public async Task ExistsAsync_ReturnsTrueOnlyForExistingMember()
	{
		var member = _db.AddMember(iin: 900101300123);

		Assert.True(await CreateService().ExistsAsync(member.IIN));
		Assert.False(await CreateService().ExistsAsync(111111111111));
	}

	[Fact]
	public async Task GetAllAsync_ReturnsAllMembers()
	{
		_db.AddMember(iin: 900101300001);
		_db.AddMember(iin: 900101300002);

		var members = await CreateService().GetAllAsync();

		Assert.Equal(2, members.Count);
	}

	[Fact]
	public async Task GetByIinAsync_UnknownMember_ReturnsNull()
	{
		Assert.Null(await CreateService().GetByIinAsync(111111111111));
	}

	[Fact]
	public async Task CreateAsync_SavesMemberAndCalculatesAge()
	{
		var birthDay = DateTime.Today.AddYears(-30);
		var request = new CreateMemberRequest(900101300123, "Ivan", "Ivanov", null, birthDay, "Almaty", "+7(777) 123-4567");

		await CreateService().CreateAsync(request);

		var saved = await CreateService().GetByIinAsync(900101300123);
		Assert.NotNull(saved);
		Assert.Equal(30, saved.Age);
		Assert.Equal("None", saved.Patronymic);
	}

	[Fact]
	public async Task UpdateAsync_ChangesFields()
	{
		var member = _db.AddMember();
		var request = new UpdateMemberRequest("Petr", "Petrov", "Ivanovich", new DateTime(1990, 1, 1), "Astana", "+7(777) 000-0000");

		await CreateService().UpdateAsync(member.IIN, request);

		var saved = await CreateService().GetByIinAsync(member.IIN);
		Assert.Equal("Petr", saved!.Name);
		Assert.Equal("Astana", saved.Address);
	}

	[Fact]
	public async Task UpdateAsync_UnknownMember_ReturnsNull()
	{
		var request = new UpdateMemberRequest("Petr", "Petrov", null, new DateTime(1990, 1, 1), "Astana", "+7(777) 000-0000");

		Assert.Null(await CreateService().UpdateAsync(111111111111, request));
	}

	[Fact]
	public async Task DeleteAsync_RemovesMember()
	{
		var member = _db.AddMember();

		Assert.True(await CreateService().DeleteAsync(member.IIN));
		Assert.False(await CreateService().ExistsAsync(member.IIN));
	}

	[Fact]
	public async Task DeleteAsync_UnknownMember_ReturnsFalse()
	{
		Assert.False(await CreateService().DeleteAsync(111111111111));
	}
}
