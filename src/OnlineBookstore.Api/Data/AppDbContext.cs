using Microsoft.EntityFrameworkCore;
using OnlineBookstore.Api.Entities;

namespace OnlineBookstore.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<CustomerAddress> CustomerAddresses => Set<CustomerAddress>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<CouponRedemption> CouponRedemptions => Set<CouponRedemption>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<EmailOutbox> EmailOutbox => Set<EmailOutbox>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>().HasIndex(x => x.UserName).IsUnique();
        modelBuilder.Entity<AppUser>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Book>().HasIndex(x => x.Isbn).IsUnique();
        modelBuilder.Entity<Category>().HasIndex(x => x.Slug).IsUnique();
        modelBuilder.Entity<Coupon>().HasIndex(x => x.Code).IsUnique();
        modelBuilder.Entity<Order>().HasIndex(x => x.OrderNumber).IsUnique();

        modelBuilder.Entity<Book>().Property(x => x.Price).HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(x => x.Subtotal).HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(x => x.DiscountAmount).HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(x => x.ShippingFee).HasPrecision(18, 2);
        modelBuilder.Entity<Order>().Property(x => x.TotalAmount).HasPrecision(18, 2);
        modelBuilder.Entity<OrderItem>().Property(x => x.UnitPrice).HasPrecision(18, 2);
        modelBuilder.Entity<OrderItem>().Property(x => x.LineTotal).HasPrecision(18, 2);
        modelBuilder.Entity<Payment>().Property(x => x.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<Coupon>().Property(x => x.Value).HasPrecision(18, 2);
        modelBuilder.Entity<Coupon>().Property(x => x.MinimumOrderAmount).HasPrecision(18, 2);
        modelBuilder.Entity<CouponRedemption>().Property(x => x.DiscountAmount).HasPrecision(18, 2);

        modelBuilder.Entity<CartItem>()
            .HasIndex(x => new { x.AppUserId, x.BookId })
            .IsUnique();

        modelBuilder.Entity<WishlistItem>()
            .HasIndex(x => new { x.AppUserId, x.BookId })
            .IsUnique();
    }
}
