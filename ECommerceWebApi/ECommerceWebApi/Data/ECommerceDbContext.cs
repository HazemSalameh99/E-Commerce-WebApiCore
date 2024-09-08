using ECommerceWebApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWebApi.Data
{
    public class ECommerceDbContext:IdentityDbContext<ApplicationUser>
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options):base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShippingAddress> ShippingAddresses { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.ShoppingCart)
                .WithOne(c => c.User)
                .HasForeignKey<ShoppingCart>(c => c.UserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.ShippingAddress)
                .WithOne(a => a.User)
                .HasForeignKey(a=>a.UserId);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p=>p.Category)
                .HasForeignKey(c=>c.CategoryId);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.OrderItems)
                .WithOne(o => o.Product)
                .HasForeignKey(oi=>oi.ProductId);

            modelBuilder.Entity<ShoppingCart>()
                .HasMany(s => s.ShoppingCartItems)
                .WithOne(si => si.ShoppingCart)
                .HasForeignKey(si => si.ShoppingCartId);

            modelBuilder.Entity<Product>()
                .HasMany(p=>p.ShoppingCartItem)
                .WithOne(s=>s.Product)
                .HasForeignKey(s=>s.ProductId);

           

              
                
        }

    }
}
