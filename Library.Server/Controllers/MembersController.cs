using Microsoft.AspNetCore.Mvc;

namespace Library.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    // TODO: GET    api/members?search=&page=&pageSize=  -> paged list of MemberDto
    // TODO: GET    api/members/{iin}
    // TODO: GET    api/members/{iin}/photo               -> photo as a separate endpoint
    // TODO: POST   api/members
    // TODO: PUT    api/members/{iin}
    // TODO: DELETE api/members/{iin}
}
