
using Microsoft.EntityFrameworkCore;
using DATENOW18062026_BE_NgoThiNgocHuyen.Models;
namespace DATENOW18062026_BE_NgoThiNgocHuyen.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly AppDbContext _context;
        public PetRepository(AppDbContext context)
        {
            _context = context;
        }
        // GET ALL
        public async Task<List<Pets>> GetAll()
        {
            return await _context.Pets
            .Include(f => f.PetCategory)
            .ToListAsync();
        }
        // GET BY ID
        public async Task<Pets?> GetById(int id)
        {
            return await _context.Pets
            .Include(f => f.PetCategory)
            .FirstOrDefaultAsync(f => f.Id == id);
        }
        // ADD
        public async Task Add(Pets pet)
        {
            // Validate Category FK
            var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == pet.CategoryId);
            if (!categoryExists)
                throw new Exception("Invalid CategoryId");
            // Validate CHECK constraint (Price)
            if (pet.Price <= 0)
                throw new Exception("Price must be greater than 0");
            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

        }
        public async Task Update(Pets pet)
        {
            var existing = await _context.Pets
            .FirstOrDefaultAsync(f => f.Id == pet.Id);
            if (existing == null)
                throw new Exception("Pet not found");
            // Validate Category FK
            var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == pet.CategoryId);
            if (!categoryExists)
                throw new Exception("Invalid CategoryId");
            // Update fields (SAFE)
            existing.PetsName = pet.PetsName;
            existing.Price = pet.Price;
            existing.StockQuantity = pet.StockQuantity;
            existing.CategoryId = pet.CategoryId;
            await _context.SaveChangesAsync();
        }
        // DELETE
        public async Task Delete(int id)
        {
            var fruit = await _context.Pets
            .FirstOrDefaultAsync(f => f.Id == id);
            if (fruit == null)
                return;
            _context.Pets.Remove(fruit);
            await _context.SaveChangesAsync();
        }
        // GET BY CATEGORY
        public async Task<List<Pets>> GetByCategoryId(int categoryId)
        {
            return await _context.Pets
            .Include(f => f.PetCategory)
            .Where(f => f.CategoryId == categoryId)
            .ToListAsync();
        }
    }
}
