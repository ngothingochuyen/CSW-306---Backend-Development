using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lab5.Data;
using Lab5.Models; // Đảm bảo đúng thư mục chứa model Category của bạn (nếu có từ Lab 4)

namespace Lab5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize(Policy = "ManageActiveCategories")] 
    public IActionResult AddCategory([FromBody] CategoryMock model)
    {
        return Ok(new
        {
            Message = "Category added successfully! Policy 'ManageActiveCategories' passed.",
            Category = model
        });
    }
}

public class CategoryMock
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}