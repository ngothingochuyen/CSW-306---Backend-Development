using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Lab5.Data;

namespace Lab5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;

    public BooksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("upload")]
    [Authorize(Policy = "VerifiedEmailOnly")] 
    public async Task<IActionResult> UploadBookFiles(IFormFile pdf, IFormFile cover)
    {
        if (pdf == null || cover == null)
            return BadRequest("Files are required.");

        return Ok("Files uploaded successfully!");
    }
}