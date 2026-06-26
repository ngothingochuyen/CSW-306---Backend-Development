using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Models;

namespace Lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BooksController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetBooks(
            [FromQuery] string? search,
            [FromQuery] int? categoryId,
            [FromQuery] int? authorId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            var query = _context.Books.Where(b => !b.IsDeleted).AsQueryable();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(b => b.Title.Contains(search));

            if (categoryId.HasValue) query = query.Where(b => b.CategoryId == categoryId);
            if (authorId.HasValue) query = query.Where(b => b.AuthorId == authorId);

            var totalItems = await query.CountAsync();
            var books = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return Ok(new { totalItems, page, pageSize, data = books });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null || book.IsDeleted) return NotFound();
            return book;
        }

        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromForm] Book book, IFormFile? coverImage, IFormFile? pdfFile)
        {
            if (coverImage != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(coverImage.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/covers", fileName);
                using (var stream = new FileStream(path, FileMode.Create)) await coverImage.CopyToAsync(stream);
                book.CoverImageUrl = "/covers/" + fileName;
            }

            if (pdfFile != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(pdfFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdfs", fileName);
                using (var stream = new FileStream(path, FileMode.Create)) await pdfFile.CopyToAsync(stream);
                book.PdfUrl = "/pdfs/" + fileName;
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.BookId) return BadRequest();
            _context.Entry(book).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!_context.Books.Any(e => e.BookId == id)) return NotFound(); else throw; }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            book.IsDeleted = true; 
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}/read")]
        public async Task<IActionResult> ReadBookPdf(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null || book.IsDeleted)
            {
                return NotFound(new { message = "Không tìm thấy cuốn sách này." });
            }

            if (string.IsNullOrEmpty(book.PdfUrl))
            {
                return BadRequest(new { message = "Cuốn sách này hiện tại chưa có file PDF để đọc online." });
            }

            return Ok(new
            {
                BookId = book.BookId,
                Title = book.Title,
                PdfRelativeUrl = book.PdfUrl
            });

        }

        [HttpGet("deleted")]
        public async Task<ActionResult<IEnumerable<Book>>> GetDeletedBooks()
        {
            var deletedBooks = await _context.Books
                .Where(b => b.IsDeleted == true)
                .ToListAsync();

            return Ok(deletedBooks);
        }

        [HttpPatch("{id}/restore")]
        public async Task<IActionResult> RestoreBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
                return NotFound(new { message = "Không tìm thấy cuốn sách này trong hệ thống." });

            if (!book.IsDeleted)
                return BadRequest(new { message = "Cuốn sách này hiện không nằm trong thùng rác." });

            book.IsDeleted = false;

            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã khôi phục thành công cuốn sách '{book.Title}' từ thùng rác." });
        }

        [HttpDelete("{id}/hard")]
        public async Task<IActionResult> HardDeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
                return NotFound(new { message = "Không tìm thấy cuốn sách cần xóa vĩnh viễn." });

            if (!string.IsNullOrEmpty(book.PdfUrl))
            {
                var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.PdfUrl.TrimStart('/'));
                if (System.IO.File.Exists(pdfPath))
                {
                    System.IO.File.Delete(pdfPath);
                }
            }

            if (!string.IsNullOrEmpty(book.CoverImageUrl))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.CoverImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Đã xóa vĩnh viễn cuốn sách '{book.Title}' và toàn bộ file liên quan khỏi hệ thống." });
        }
    }
}