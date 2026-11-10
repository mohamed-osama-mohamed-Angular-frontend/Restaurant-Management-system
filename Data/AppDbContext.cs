using Microsoft.EntityFrameworkCore;
using Shawermawy.Models;

namespace Shawermawy.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ── Primary Keys ──────────────────────────────────────────
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Category>().HasKey(c => c.CategoryId);
            modelBuilder.Entity<MenuItem>().HasKey(m => m.ItemId);
            modelBuilder.Entity<CartItem>().HasKey(c => c.CartItemId);
            modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
            modelBuilder.Entity<OrderItem>().HasKey(oi => oi.OrderItemId);
            modelBuilder.Entity<Review>().HasKey(r => r.ReviewId);

            // ── Decimal Precision ─────────────────────────────────────
            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(10,2)");

            // ── MenuItem → Category ───────────────────────────────────
            modelBuilder.Entity<MenuItem>()
                .HasOne(m => m.Category)
                .WithMany(c => c.MenuItems)
                .HasForeignKey(m => m.CategoryId);

            // ── CartItem → User ───────────────────────────────────────
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(ci => ci.UserId);

            // ── CartItem → MenuItem ───────────────────────────────────
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.MenuItem)
                .WithMany()
                .HasForeignKey(ci => ci.ItemId);  // ← fixes MenuItemItemId

            // ── Order → User ──────────────────────────────────────────
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            // ── OrderItem → Order ─────────────────────────────────────
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            // ── OrderItem → MenuItem ──────────────────────────────────
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuItem)
                .WithMany()
                .HasForeignKey(oi => oi.ItemId);  // ← fixes MenuItemItemId

            // ── Review → User ─────────────────────────────────────────
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);
        }
    }
}