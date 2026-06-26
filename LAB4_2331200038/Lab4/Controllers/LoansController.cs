using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Models;

namespace Lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly LibraryContext _context;

        public LoansController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetLoans(
            [FromQuery] int? userId,
            [FromQuery] string? status,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate)
        {
            var query = _context.Loans.Include(l => l.Book).Include(l => l.User).AsQueryable();

            if (userId.HasValue) query = query.Where(l => l.UserId == userId);
            if (!string.IsNullOrEmpty(status)) query = query.Where(l => l.Status == status);
            if (fromDate.HasValue) query = query.Where(l => l.LoanDate >= fromDate);
            if (toDate.HasValue) query = query.Where(l => l.LoanDate <= toDate);

            return Ok(await query.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(int id)
        {
            var loan = await _context.Loans.Include(l => l.Book).Include(l => l.User).FirstOrDefaultAsync(l => l.LoanId == id);
            if (loan == null) return NotFound();
            return loan;
        }

        [HttpPost]
        public async Task<ActionResult<Loan>> PostLoan(Loan loan)
        {
            var book = await _context.Books.FindAsync(loan.BookId);
            if (book == null) return NotFound("Không tìm thấy sách.");

            if (book.AvailableCopies <= 0)
            {
                return BadRequest("Sách đã hết bản sao, không thể cho mượn.");
            }

            book.AvailableCopies -= 1;

            loan.LoanDate = DateTime.Now;
            loan.Status = "Borrowed";
            if (loan.DueDate == default) loan.DueDate = DateTime.Now.AddDays(14); 

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoan", new { id = loan.LoanId }, loan);
        }

        [HttpPut("return/{id}")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var loan = await _context.Loans.Include(l => l.Book).FirstOrDefaultAsync(l => l.LoanId == id);
            if (loan == null) return NotFound();

            if (loan.Status == "Returned") return BadRequest("Phiếu mượn này đã được trả rồi.");

            loan.ReturnDate = DateTime.Now;
            loan.Status = "Returned";

            if (loan.Book != null)
            {
                loan.Book.AvailableCopies += 1;
            }

            await _context.SaveChangesAsync();
            return Ok(new { Message = "Trả sách thành công!", ReturnDate = loan.ReturnDate });
        }
    }
}