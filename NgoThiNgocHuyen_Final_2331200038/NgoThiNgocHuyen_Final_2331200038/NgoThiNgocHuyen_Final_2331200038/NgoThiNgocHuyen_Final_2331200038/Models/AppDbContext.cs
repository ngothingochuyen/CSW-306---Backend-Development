using Microsoft.EntityFrameworkCore;

namespace DATENOW18062026_BE_NgoThiNgocHuyen.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        { }
        public AppDbContext(DbContextOptions<AppDbContext>
        options)
            : base(options)
        { }
        public virtual DbSet<PetCategory> Categories { get; set; }
        public virtual DbSet<Pets> Pets { get; set; }
        public virtual DbSet<PurchaseOrders> Orders { get; set; }
        public virtual DbSet<PurchaseItems> PurchaseItems { get; set; }
        public virtual DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PetCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            });
            modelBuilder.Entity<Pets>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StockQuantity).HasDefaultValue(0.0);
                entity.HasOne(d => d.PetCategory).WithMany(p => p.Pets)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<PurchaseOrders>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.HasOne(d => d.User).WithMany(p => p.Orders)
            .OnDelete(DeleteBehavior.ClientSetNull);
            });
            modelBuilder.Entity<PurchaseItems>(entity =>
            {
                entity.HasKey(e => e.Id).HasName
                ("PK__OrderIte__3214EC07B341EC31");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.HasOne(d => d.Pet).WithMany(p => p.PurchaseItems)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__OrderItem__Fruit__4D94879B");
                entity.HasOne(d => d.PurchaseOrders).WithMany(p => p.PurchaseItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderItem__Order__4CA06362");
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName
                ("PK__Users__3214EC07D9401F1B");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(u => u.Role).HasConversion<string>();
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
