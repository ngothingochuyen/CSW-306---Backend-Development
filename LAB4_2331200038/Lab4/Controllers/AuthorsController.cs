using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Models;
using System.IO;

namespace Lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public AuthorsController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors([FromQuery] string? name)
        {
            var query = _context.Authors.Where(a => !a.IsDeleted).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(a => a.FirstName.Contains(name) || a.LastName.Contains(name));
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null || author.IsDeleted) return NotFound();
            return author;
        }

        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor([FromForm] Author author, IFormFile? avatarFile)
        {
            if (avatarFile != null && avatarFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(avatarFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/avatars", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await avatarFile.CopyToAsync(stream);
                }
                author.AvatarUrl = "/avatars/" + fileName;
            }

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetAuthor", new { id = author.AuthorId }, author);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, [FromForm] Author author, IFormFile? avatarFile)
        {
            if (id != author.AuthorId) return BadRequest();

            if (avatarFile != null && avatarFile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(avatarFile.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/avatars", fileName);
                using (var stream = new FileStream(path, FileMode.Create)) await avatarFile.CopyToAsync(stream);
                author.AvatarUrl = "/avatars/" + fileName;
            }

            _context.Entry(author).State = EntityState.Modified;
            _context.Entry(author).Property(x => x.IsDeleted).IsModified = false;

            try { await _context.SaveChangesAsync(); }
            catch (DbUpdateConcurrencyException) { if (!AuthorExists(id)) return NotFound(); else throw; }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return NotFound();

            author.IsDeleted = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.AuthorId == id);
        }
    }
}