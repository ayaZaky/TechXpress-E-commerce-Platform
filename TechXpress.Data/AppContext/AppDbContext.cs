using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechXpress.Data.Entities;


namespace TechXpress.Data.AppContext
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } // Chaining Constructor
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<WishListItem> WishListItems { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); 
              
            // Configure relationships
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            // ProductImages - Product (Many-to-One)
            builder.Entity<Product>()
               .HasMany(p => p.Images)
               .WithOne(pi => pi.Product)
               .HasForeignKey(pi => pi.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
            // OrderItem - Order (Many-to-One)
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            // OrderItem - Product (Many-to-One)
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            // CartItem - Product (Many-to-One)
            builder.Entity<CartItem>()
                .HasOne(c => c.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            // Configure composite unique constraint
            builder.Entity<CartItem>()
                .HasIndex(c => new { c.UserId, c.ProductId })
                .IsUnique();
            // Address - User (Many-to-One)
            builder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // PaymentMethod - User (Many-to-One)
            builder.Entity<PaymentMethod>()
                .HasOne(pm => pm.User)
                .WithMany(u => u.PaymentMethods)
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // PaymentTransaction - Order (Many-to-One)
            builder.Entity<PaymentTransaction>()
                .HasOne(pt => pt.Order)
                .WithMany(o => o.PaymentTransactions)
                .HasForeignKey(pt => pt.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            // PaymentTransaction - PaymentMethod (Many-to-One)
            builder.Entity<PaymentTransaction>()
                .HasOne(pt => pt.PaymentMethod)
                .WithMany(pm => pm.PaymentTransactions)
                .HasForeignKey(pt => pt.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);
            // Refund - PaymentTransaction (Many-to-One)
            builder.Entity<Refund>()
                .HasOne(r => r.PaymentTransaction)
                .WithMany(pt => pt.Refunds)
                .HasForeignKey(r => r.PaymentTransactionId)
                .OnDelete(DeleteBehavior.Cascade);


            // WishlistItem - User (Many-to-One)
            builder.Entity<WishListItem>()
                .HasOne(w => w.User)
                .WithMany(u => u.WishItems)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // WishlistItem - Product (Many-to-One)
            builder.Entity<WishListItem>()
                .HasOne(w => w.Product)
                .WithMany(p => p.WishItems)
                .HasForeignKey(w => w.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Review - User (Many-to-One)
            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Review - Product (Many-to-One)
            builder.Entity<Review>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
