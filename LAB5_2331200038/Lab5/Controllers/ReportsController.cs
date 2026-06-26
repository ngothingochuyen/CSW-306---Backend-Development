using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lab5.Data; 

namespace Lab5.Controllers; 

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("top-borrowed")]
    [Authorize(Policy = "AdminOrLibrarian")] 
    public IActionResult GetTopBorrowedBooks([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate, [FromQuery] int top = 10)
    {
        var report = _context.Loans
            .Where(l => l.LoanDate >= fromDate && l.LoanDate <= toDate)
            .GroupBy(l => l.BookId)
            .Select(g => new {
                BookId = g.Key,
                Title = _context.Books.FirstOrDefault(b => b.BookId == g.Key).Title,
                TimesBorrowed = g.Count()
            })
            .OrderByDescending(x => x.TimesBorrowed)
            .Take(top)
            .ToList();

        return Ok(report);
    }
}