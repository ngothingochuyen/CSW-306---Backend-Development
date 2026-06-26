using Microsoft.EntityFrameworkCore;
using _06_12_2026_BE_NgoThiNgocHuyen.Models;

namespace _06_12_2026_BE_NgoThiNgocHuyen.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }

        public DbSet<Categories> Categories { get; set; }

        public DbSet<Fruits> Fruits { get; set; }

        public DbSet<Orders> Orders { get; set; }

        public DbSet<OrderItems> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .Property(u => u.Role)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
