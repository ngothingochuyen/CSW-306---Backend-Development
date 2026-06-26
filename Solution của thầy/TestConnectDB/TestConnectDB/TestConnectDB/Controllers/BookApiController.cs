using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestConnectDB.Models;

namespace TestConnectDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookApiController : ControllerBase
    {
        private readonly BookManagementContext _context;

        public BookApiController(BookManagementContext context)
        {
            _context = context;
        }

        // Lấy danh sách sách
        [HttpGet("ListBooks")]
        public IActionResult Index()
        {
            var books = _context.Book.ToList();
            return Ok(books);
        }

    }
}
