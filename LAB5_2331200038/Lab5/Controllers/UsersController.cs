using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Lab5.Data;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetMe()
    {
        var email = User.Identity?.Name;

        var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

        return Ok(new
        {
            email,
            roles
        });
    }

    [HttpGet("active-list")]
    [Authorize(Policy = "ActiveUserOnly")]
    public IActionResult GetActiveUsers()
    {
        var activeUsers = _context.Users
            .Where(u => u.IsActive)
            .Select(u => new { u.UserId, u.Email, u.IsActive })
            .ToList();

        return Ok(new
        {
            Message = "Access granted to active user!",
            Data = activeUsers
        });
    }
}