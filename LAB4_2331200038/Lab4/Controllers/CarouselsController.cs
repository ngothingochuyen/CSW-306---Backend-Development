using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Models;

namespace Lab4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarouselsController : ControllerBase
    {
        private readonly LibraryContext _context;

        public CarouselsController(LibraryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Carousel>>> GetCarousels()
        {
            return await _context.Carousels.OrderBy(c => c.Order).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Carousel>> GetCarousel(int id)
        {
            var carousel = await _context.Carousels.FindAsync(id);

            if (carousel == null)
            {
                return NotFound();
            }

            return carousel;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarousel(int id, Carousel carousel)
        {
            if (id != carousel.CarouselId)
            {
                return BadRequest();
            }

            carousel.UpdatedDate = DateTime.Now;
            _context.Entry(carousel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarouselExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Carousel>> PostCarousel(Carousel carousel)
        {
            carousel.CreatedDate = DateTime.Now;

            _context.Carousels.Add(carousel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarousel", new { id = carousel.CarouselId }, carousel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarousel(int id)
        {
            var carousel = await _context.Carousels.FindAsync(id);
            if (carousel == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(carousel.ImageUrl))
            {
                try
                {
                    var relativePath = carousel.ImageUrl.TrimStart('/');
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Không thể xóa file: {ex.Message}");
                }
            }

            _context.Carousels.Remove(carousel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarouselExists(int id)
        {
            return _context.Carousels.Any(e => e.CarouselId == id);
        }
    }
}