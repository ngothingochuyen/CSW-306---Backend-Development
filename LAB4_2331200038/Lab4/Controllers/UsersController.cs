using BCrypt.Net;
using Lab4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly LibraryContext _context;

        public UsersController(LibraryContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UserRegisterDto request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest("Email đã tồn tại trong hệ thống.");

            var user = new User
            {
                Fullname = request.Fullname,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),

                ActiveCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(), 
                UserCode = "USER-" + Guid.NewGuid().ToString().Substring(0, 4).ToUpper(),

                IsActive = false,
                IsDeleted = false,
                IsLocked = false,
                Status = 1,
                CreatedDate = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            Console.WriteLine($"[EMAIL SIMULATION] Gửi tới: {user.Email} | Mã kích hoạt của bạn là: {user.ActiveCode}");

            return Ok(new
            {
                Message = "Đăng ký thành công! Vui lòng kiểm tra mã kích hoạt.",
                Email = user.Email,
                Note = "Mã kích hoạt đã được in ra cửa sổ Console/Output của Visual Studio."
            });
        }

        [HttpPost("activate")]
        public async Task<IActionResult> Activate([FromBody] ActivationRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.ActiveCode == request.Code);

            if (user == null)
                return BadRequest("Mã kích hoạt không đúng hoặc đã hết hạn.");

            if (user.IsActive)
                return BadRequest("Tài khoản này đã được kích hoạt rồi.");

            user.IsActive = true;
            user.ActiveCode = null; 

            await _context.SaveChangesAsync();

            return Ok("Tài khoản đã được kích hoạt thành công. Bạn có thể đăng nhập.");
        }
    }

    public class UserRegisterDto
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ActivationRequest
    {
        public string Code { get; set; }
    }
}