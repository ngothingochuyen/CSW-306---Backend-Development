using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Lab5.Data;

namespace Lab5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoansController : ControllerBase
{
    private readonly AppDbContext _context;

    public LoansController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("history")]
    [Authorize(Policy = "MinimumMembership")] 
    public IActionResult GetLoanHistory()
    {
        var username = User.Identity?.Name;
        var user = _context.Users.FirstOrDefault(u => u.Email == username);

        if (user == null) return Unauthorized("User not found.");

        var loans = _context.Loans.Where(l => l.UserId == user.UserId).ToList();

        return Ok(new
        {
            Message = "Access granted to long-term user!",
            Data = loans
        });
    }
}