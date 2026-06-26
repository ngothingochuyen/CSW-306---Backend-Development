using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testFinal.DTOs;
using testFinal.Models;

namespace testFinal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Sử dụng trực tiếp DbContext hoặc qua Repository đều được
        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            // Trả về danh sách tất cả các category
            return await _context.Categories.ToListAsync();
        }
    }
}