using Microsoft.EntityFrameworkCore;
using testFinal.Models;

namespace testFinal.Repositories
{
    public class FruitRepository : IFruitRepository
    {
        private readonly ApplicationDbContext _context;

        public FruitRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET ALL
        public async Task<List<Fruit>> GetAll()
        {
            return await _context.Fruits
                .Include(f => f.Category)
                .ToListAsync();
        }

        // GET BY ID
        public async Task<Fruit?> GetById(int id)
        {
            return await _context.Fruits
                .Include(f => f.Category)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        // ADD
        public async Task Add(Fruit fruit)
        {
            // Validate Category FK
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == fruit.CategoryId);

            if (!categoryExists)
                throw new Exception("Invalid CategoryId");

            // Validate CHECK constraint (Price)
            if (fruit.Price <= 0)
                throw new Exception("Price must be greater than 0");

            _context.Fruits.Add(fruit);
            await _context.SaveChangesAsync();
        }

        // UPDATE
        public async Task Update(Fruit fruit)
        {
            var existing = await _context.Fruits
                .FirstOrDefaultAsync(f => f.Id == fruit.Id);

            if (existing == null)
                throw new Exception("Fruit not found");

            // Validate Category FK
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == fruit.CategoryId);

            if (!categoryExists)
                throw new Exception("Invalid CategoryId");

            // Update fields (SAFE)
            existing.FruitsName = fruit.FruitsName;
            existing.Price = fruit.Price;
            existing.StockQuantity = fruit.StockQuantity;
            existing.CategoryId = fruit.CategoryId;

            await _context.SaveChangesAsync();
        }

        // DELETE
        public async Task Delete(int id)
        {
            var fruit = await _context.Fruits
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fruit == null)
                return;

            _context.Fruits.Remove(fruit);
            await _context.SaveChangesAsync();
        }

        // GET BY CATEGORY
        public async Task<List<Fruit>> GetByCategoryId(int categoryId)
        {
            return await _context.Fruits
                .Include(f => f.Category)
                .Where(f => f.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}