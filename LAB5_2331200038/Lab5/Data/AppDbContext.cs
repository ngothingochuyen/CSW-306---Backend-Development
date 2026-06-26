using Lab5.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Lab5.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // Optional: seed data
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, Name = "Admin", Description = "System Administrator" },
                new Role { RoleId = 2, Name = "User", Description = "Normal User" }
            );

            /////
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Fullname = "Hihi Hehe Admin",
                    Email = "admin@gmail.com", 
                    Password = "123",          
                    IsActive = true,
                    EmailConfirmed = true,
                    Status = 1
                }
            );

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { UserId = 1, RoleId = 1 }
            );
        }

    }
}
