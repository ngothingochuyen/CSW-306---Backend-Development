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