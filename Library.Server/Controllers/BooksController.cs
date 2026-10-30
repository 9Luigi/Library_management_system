using Microsoft.AspNetCore.Mvc;

namespace Library.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    // TODO: inject LibraryContext via the constructor (DI)

    // TODO: GET api/books            -> list of BookDto
    // TODO: GET api/books/{id}       -> BookDto or 404
}
