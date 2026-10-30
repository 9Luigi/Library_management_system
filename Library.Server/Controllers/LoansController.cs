using Microsoft.AspNetCore.Mvc;

namespace Library.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    // TODO: POST api/loans/borrow   (memberIin, bookId) -> one transaction, check Amount, no negative stock
    // TODO: POST api/loans/return   (memberIin, bookId)
}
