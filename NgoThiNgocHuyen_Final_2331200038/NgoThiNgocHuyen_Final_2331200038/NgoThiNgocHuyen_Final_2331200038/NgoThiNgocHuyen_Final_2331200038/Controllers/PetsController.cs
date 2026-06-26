using DATENOW18062026_BE_NgoThiNgocHuyen.Models;
using DATENOW18062026_BE_NgoThiNgocHuyen.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DATENOW18062026_BE_NgoThiNgocHuyen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IPetRepository _repo;
        public PetsController(IPetRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pets = await _repo.GetAll();
            var result = pets.Select(f => new Pets
            {
                Id = f.Id,
                PetsName = f.PetsName,
                Breed = f.Breed,
                Age = f.Age,
                Price = f.Price,
                StockQuantity = f.StockQuantity,
                CategoryId = f.CategoryId,
                CreatedAt = f.CreatedAt
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
                public async Task<IActionResult> GetById(int id)
                {
                    var pet = await _repo.GetById(id);
                    if (pet == null) return NotFound(new
                    {
                        message = "pet not found" });
        return Ok(new Pets
        {
            Id = pet.Id,
            PetsName = pet.PetsName,
            Breed = pet.Breed,
            Age = pet.Age,
            Price = pet.Price,
            StockQuantity = pet.StockQuantity,
            CategoryId = pet.CategoryId,
            CreatedAt = pet.CreatedAt
        });
                }
        // POST: api/pets - Admin Only
        [HttpPost]
        public async Task<IActionResult> Create(Pets dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var pet = new Pets
            {
                PetsName = dto.PetsName,
                Breed = dto.Breed,
                Age = dto.Age,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                CategoryId = dto.CategoryId
            };
            await _repo.Add(pet);
            return Ok(new { message = "Created successfully" });
        }
        // PUT: api/pets/{id} - Admin Only
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Pets dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var pet = await _repo.GetById(id);
            if (pet == null) return NotFound(new
            {
                message = "pet not found"
            });
            pet.PetsName = dto.PetsName;
            pet.Price = dto.Price;
            pet.StockQuantity = dto.StockQuantity;
            pet.CategoryId = dto.CategoryId;
            await _repo.Update(pet);
            return Ok(new { message = "Updated successfully" });
        }
        // DELETE: api/pets/{id} - Admin Only
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pet = await _repo.GetById(id);
            if (pet == null) return NotFound(new
            {
                message = "pet not found" });
            await _repo.Delete(id);
            return Ok(new { message = "Deleted successfully" });
        }
    }
}
