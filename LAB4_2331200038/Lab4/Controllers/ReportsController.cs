using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Models;

namespace Lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public ReportsController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet("top-borrowed")]
        public async Task<IActionResult> GetTopBorrowedBooks(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int top = 10) 
        {
            var query = _context.Loans.Include(l => l.Book).AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(l => l.LoanDate >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                query = query.Where(l => l.LoanDate <= toDate.Value);
            }

            var reportData = await query
                .GroupBy(l => new { l.BookId, Title = l.Book != null ? l.Book.Title : "Sách đã bị xóa" })
                .Select(g => new TopBorrowedBookDto
                {
                    BookId = g.Key.BookId,
                    Title = g.Key.Title,
                    BorrowCount = g.Count() 
                })
                .OrderByDescending(x => x.BorrowCount)
                .Take(top) 
                .ToListAsync();

            return Ok(reportData);
        }
    }

    public class TopBorrowedBookDto
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int BorrowCount { get; set; }
    }
}