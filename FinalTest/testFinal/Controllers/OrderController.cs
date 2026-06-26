using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using testFinal.DTOs;
using testFinal.Models;
using testFinal.Repositories;

namespace testFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Tất cả endpoint đều cần đăng nhập
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ============================================================
        // GET /api/orders
        // Lấy toàn bộ đơn hàng kèm OrderItems
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Fruit) // Lấy thêm thông tin Fruit trong mỗi item
                .Select(o => new
                {
                    o.Id,
                    o.UserId,
                    o.OrderDate,
                    o.TotalAmount,
                    o.Status,
                    o.CreatedAt,
                    OrderItems = o.OrderItems.Select(oi => new
                    {
                        oi.Id,
                        oi.FruitId,
                        FruitName = oi.Fruit.FruitsName,
                        oi.Quantity,
                        oi.UnitPrice,
                        SubTotal = oi.Quantity * oi.UnitPrice // Tính tạm để tiện xem
                    })
                })
                .ToListAsync();

            return Ok(orders);
        }

        // ============================================================
        // POST /api/orders
        // Tạo đơn hàng mới
        // Request body: { "items": [{"fruitId": 1, "quantity": 5}, ...] }
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO dto)
        {
            // 1. Lấy userId từ JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("Không tìm thấy thông tin người dùng trong token.");

            int userId = int.Parse(userIdClaim);

            // 2. Validate từng item trong đơn hàng
            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (var item in dto.Items)
            {
                // Validate: quantity > 0
                if (item.Quantity <= 0)
                    return BadRequest($"Quantity của FruitId {item.FruitId} phải lớn hơn 0.");

                // Validate: fruitId tồn tại
                var fruit = await _context.Fruits.FindAsync(item.FruitId);
                if (fruit == null)
                    return NotFound($"Không tìm thấy Fruit với Id = {item.FruitId}.");

                // Validate: stock đủ không
                if (fruit.StockQuantity == null || fruit.StockQuantity < item.Quantity)
                    return BadRequest($"Fruit '{fruit.FruitsName}' không đủ hàng trong kho. Còn lại: {fruit.StockQuantity ?? 0}.");

                // Lấy giá hiện tại của Fruit làm UnitPrice
                decimal unitPrice = fruit.Price;
                decimal subTotal = unitPrice * item.Quantity;
                totalAmount += subTotal;

                orderItems.Add(new OrderItem
                {
                    FruitId = item.FruitId,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    CreatedAt = DateTime.Now
                });

                // Trừ stock sau khi đặt hàng
                fruit.StockQuantity -= item.Quantity;
                _context.Fruits.Update(fruit);
            }

            // 3. Tạo Order
            var order = new Order
            {
                UserId = userId,          // Lấy từ JWT
                OrderDate = DateTime.Now, // Server tự set
                Status = "Pending",       // Mặc định
                TotalAmount = totalAmount,// Tính từ items
                CreatedAt = DateTime.Now,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Tạo đơn hàng thành công.",
                OrderId = order.Id,
                order.UserId,
                order.OrderDate,
                order.Status,
                order.TotalAmount,
                OrderItems = order.OrderItems.Select(oi => new
                {
                    oi.FruitId,
                    oi.Quantity,
                    oi.UnitPrice,
                    SubTotal = oi.Quantity * oi.UnitPrice
                })
            });
        }

        // ============================================================
        // GET /api/orders/user/{userId}
        // Lấy đơn hàng theo userId
        // Chỉ được xem đơn hàng của chính mình
        // ============================================================
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            // Lấy userId từ JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("Không tìm thấy thông tin người dùng trong token.");

            int tokenUserId = int.Parse(userIdClaim);

            // Validate: chỉ được xem đơn hàng của chính mình
            if (tokenUserId != userId)
                return Forbid(); // 403 — không có quyền xem của người khác

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Fruit)
                .Select(o => new
                {
                    o.Id,
                    o.UserId,
                    o.OrderDate,
                    o.TotalAmount,
                    o.Status,
                    o.CreatedAt,
                    OrderItems = o.OrderItems.Select(oi => new
                    {
                        oi.Id,
                        oi.FruitId,
                        FruitName = oi.Fruit.FruitsName,
                        oi.Quantity,
                        oi.UnitPrice,
                        SubTotal = oi.Quantity * oi.UnitPrice
                    })
                })
                .ToListAsync();

            if (!orders.Any())
                return NotFound($"Không tìm thấy đơn hàng nào của userId = {userId}.");

            return Ok(orders);
        }
    }
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using testFinal.DTOs;
using testFinal.Models;
using testFinal.Repositories;

namespace testFinal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FruitsController : ControllerBase
    {
        private readonly IFruitRepository _repo;

        public FruitsController(IFruitRepository repo)
        {
            _repo = repo;
        }

        // GET: api/fruits - Public
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fruits = await _repo.GetAll();
            var result = fruits.Select(f => new FruitDTO
            {
                Id = f.Id,
                Name = f.FruitsName,
                Price = f.Price,
                StockQuantity = f.StockQuantity,
                CategoryId = f.CategoryId,
                CategoryName = f.Category?.CategoriesName
            });
            return Ok(result);
        }

        // GET: api/fruits/{id} - Public
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var fruit = await _repo.GetById(id);
            if (fruit == null) return NotFound(new { message = "Fruit not found" });

            return Ok(new FruitDTO
            {
                Id = fruit.Id,
                Name = fruit.FruitsName,
                Price = fruit.Price,
                StockQuantity = fruit.StockQuantity,
                CategoryId = fruit.CategoryId,
                CategoryName = fruit.Category?.CategoriesName
            });
        }

        // POST: api/fruits - Admin Only
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(FruitDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var fruit = new Fruit
            {
                FruitsName = dto.Name,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId
            };

            await _repo.Add(fruit);
            return Ok(new { message = "Created successfully" });
        }

        // PUT: api/fruits/{id} - Admin Only
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, FruitDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var fruit = await _repo.GetById(id);
            if (fruit == null) return NotFound(new { message = "Fruit not found" });

            fruit.FruitsName = dto.Name;
            fruit.Price = dto.Price;
            fruit.StockQuantity = dto.StockQuantity;
            fruit.CategoryId = dto.CategoryId;

            await _repo.Update(fruit);
            return Ok(new { message = "Updated successfully" });
        }

        // DELETE: api/fruits/{id} - Admin Only
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var fruit = await _repo.GetById(id);
            if (fruit == null) return NotFound(new { message = "Fruit not found" });

            await _repo.Delete(id);
            return Ok(new { message = "Deleted successfully" });
        }
    }
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using testFinal.DTOs;
using testFinal.Models;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(ApplicationDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        // 1. Tìm user trong DB
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.PasswordHash == loginDto.Password);

        if (user == null) return Unauthorized("Invalid email or password");

        // 2. Tạo Claims (Thông tin người dùng lưu trong Token)
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        // 3. Tạo Key bảo mật (đọc từ appsettings.json)
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 4. Tạo Token
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}