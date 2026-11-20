using Library.Contracts.Members;
using Library.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController(MemberService memberService) : ControllerBase
{
	// TODO: paging (?page=&pageSize=) and search
	// TODO: GET api/members/{iin}/photo -> photo as a separate endpoint

	// GET api/members
	[HttpGet]
	public async Task<List<MemberDto>> GetAll() => await memberService.GetAllAsync();

	// GET api/members/{iin}
	[HttpGet("{iin:long}")]
	public async Task<ActionResult<MemberDto>> Get(long iin)
	{
		var member = await memberService.GetByIinAsync(iin);
		return member == null ? NotFound() : member;
	}

	// POST api/members
	[HttpPost]
	public async Task<ActionResult<MemberDto>> Create(CreateMemberRequest request)
	{
		if (await memberService.ExistsAsync(request.IIN))
			return Conflict($"Member with IIN {request.IIN} already exists.");

		var created = await memberService.CreateAsync(request);
		// 201 Created + the address of the new member in the Location header
		return CreatedAtAction(nameof(Get), new { iin = created.IIN }, created);
	}

	// PUT api/members/{iin}
	[HttpPut("{iin:long}")]
	public async Task<ActionResult<MemberDto>> Update(long iin, UpdateMemberRequest request)
	{
		var updated = await memberService.UpdateAsync(iin, request);
		return updated == null ? NotFound() : updated;
	}

	// DELETE api/members/{iin}
	[HttpDelete("{iin:long}")]
	public async Task<IActionResult> Delete(long iin)
	{
		return await memberService.DeleteAsync(iin) ? NoContent() : NotFound();
	}
}
