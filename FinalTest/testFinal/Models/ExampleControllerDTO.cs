// ============================================================
// TEMPLATE CRUD CONTROLLER — Copy và thay thế:
//   "Product"     → tên entity của bạn (Book, Fruit, Order...)
//   "ProductDTO"  → tên DTO tương ứng
//   "_context.Products" → DbSet tương ứng trong DbContext
// ============================================================

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using YourProject.DTOs;
using YourProject.Models;

namespace YourProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]            // ← Bỏ comment nếu TOÀN BỘ controller cần đăng nhập
    // [Authorize(Roles = "Admin")] // ← Bỏ comment nếu chỉ Admin mới dùng được
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }


        // ============================================================
        // GET /api/products
        // Lấy toàn bộ danh sách
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.Products
                // .Include(p => p.Category)   // ← Bỏ comment nếu cần load quan hệ
                .Select(p => new ProductResponseDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    // Map thêm field nếu cần
                    //Quy tắc đơn giản: bên trái = field của DTO, bên phải = field của Model.
                })
                .ToListAsync();

            return Ok(list);
        }


        // ============================================================
        // GET /api/products/{id}
        // Lấy 1 bản ghi theo Id
        // ============================================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _context.Products
                // .Include(p => p.Category)   // ← Bỏ comment nếu cần load quan hệ
                .Where(p => p.Id == id)
                .Select(p => new ProductResponseDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                })
                .FirstOrDefaultAsync();

            if (item == null)
                return NotFound($"Không tìm thấy Product với Id = {id}.");

            return Ok(item);
        }


        // ============================================================
        // POST /api/products
        // Tạo mới 1 bản ghi
        // ============================================================
        [HttpPost]
        // [Authorize]           // ← Bỏ comment nếu endpoint này cần đăng nhập
        // [Authorize(Roles = "Admin")] // ← Bỏ comment nếu chỉ Admin tạo được
        public async Task<IActionResult> Create([FromBody] CreateProductDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // [Lấy userId từ JWT nếu cần]
            // var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var item = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                // UserId = userId,          // ← Nếu cần gán userId từ JWT
                CreatedAt = DateTime.Now,    // ← Server tự set, không nhận từ client
            };

            _context.Products.Add(item);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Tạo thành công.", id = item.Id });
            // Hoặc trả 201 Created:
            // return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }


        // ============================================================
        // PUT /api/products/{id}
        // Cập nhật toàn bộ 1 bản ghi
        // ============================================================
        [HttpPut("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _context.Products.FindAsync(id);
            if (item == null)
                return NotFound($"Không tìm thấy Product với Id = {id}.");

            // [Kiểm tra quyền — chỉ owner mới sửa được, nếu cần]
            // var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            // if (item.UserId != userId) return Forbid();

            // Cập nhật từng field từ DTO
            item.Name = dto.Name;
            item.Price = dto.Price;
            // item.UpdatedAt = DateTime.Now; // ← Nếu có field UpdatedAt

            _context.Products.Update(item);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật thành công." });
        }


        // ============================================================
        // PATCH /api/products/{id}
        // Cập nhật 1 phần (chỉ field nào gửi lên mới đổi)
        // ============================================================
        [HttpPatch("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PartialUpdate(int id, [FromBody] PatchProductDTO dto)
        {
            var item = await _context.Products.FindAsync(id);
            if (item == null)
                return NotFound($"Không tìm thấy Product với Id = {id}.");

            // Chỉ update field nào client gửi lên (khác null)
            if (dto.Name != null) item.Name = dto.Name;
            if (dto.Price != null) item.Price = dto.Price.Value;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật một phần thành công." });
        }


        // ============================================================
        // DELETE /api/products/{id}
        // Xóa 1 bản ghi
        // ============================================================
        [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Products.FindAsync(id);
            if (item == null)
                return NotFound($"Không tìm thấy Product với Id = {id}.");

            // [Kiểm tra quyền nếu cần]
            // var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            // if (item.UserId != userId) return Forbid();

            _context.Products.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa thành công." });
        }


        // ============================================================
        // GET /api/products/search?name=apple&minPrice=10&maxPrice=100
        // Tìm kiếm + lọc (mở rộng thêm nếu cần)
        // ============================================================
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? name,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            var result = await query
                .Select(p => new ProductResponseDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                })
                .ToListAsync();

            return Ok(result);
        }

    }
}

// ============================================================
// TEMPLATE DTOs — Copy và thay thế:
//   "Product" → tên entity của bạn
//   Thêm/bớt field cho phù hợp với Model thực tế
// ============================================================

using System.ComponentModel.DataAnnotations;

namespace YourProject.DTOs
{
    // ============================================================
    // RESPONSE DTO — Dùng khi TRẢ DỮ LIỆU về cho client
    // Dùng trong: GET /api/products, GET /api/products/{id}
    // Chỉ expose field nào client được phép thấy
    // ============================================================
    public class ProductResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        // Thêm field khác nếu cần trả về
        // public string CategoryName { get; set; }  // Nếu có include Category
    }


    // ============================================================
    // CREATE DTO — Dùng khi client GỬI DỮ LIỆU LÊN để tạo mới
    // Dùng trong: POST /api/products
    // Chỉ nhận field nào client được phép tự nhập
    // Không nhận: Id, CreatedAt, UserId, Status (server tự set)
    // ============================================================
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "Tên không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Giá không được để trống.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0.")]
        public decimal Price { get; set; }

        // Thêm field khác nếu cần
        // public int CategoryId { get; set; }
        // public string Description { get; set; }
    }


    // ============================================================
    // UPDATE DTO — Dùng khi client GỬI DỮ LIỆU LÊN để cập nhật
    // Dùng trong: PUT /api/products/{id}
    // Thường giống CreateDTO, nhưng tách riêng để linh hoạt
    // ============================================================
    public class UpdateProductDTO
    {
        [Required(ErrorMessage = "Tên không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Giá không được để trống.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0.")]
        public decimal Price { get; set; }
    }


    // ============================================================
    // PATCH DTO — Dùng khi client chỉ update MỘT PHẦN
    // Dùng trong: PATCH /api/products/{id}
    // Tất cả field đều nullable — field nào null = không đổi
    // ============================================================
    public class PatchProductDTO
    {
        [StringLength(100)]
        public string? Name { get; set; }   // null = không đổi Name

        [Range(0.01, double.MaxValue)]
        public decimal? Price { get; set; } // null = không đổi Price
    }
}