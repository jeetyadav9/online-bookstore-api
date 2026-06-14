using Microsoft.EntityFrameworkCore;
using OnlineBookstore.Api.Entities;
using OnlineBookstore.Api.Enums;
using OnlineBookstore.Api.Security;

namespace OnlineBookstore.Api.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (!await db.Users.AnyAsync())
        {
            db.Users.AddRange(
                new AppUser
                {
                    UserName = "admin",
                    Email = "admin@bookstore.local",
                    FullName = "Jeet Yadav Admin",
                    PasswordHash = PasswordHasher.Hash("admin123"),
                    Role = UserRole.Admin
                },
                new AppUser
                {
                    UserName = "customer",
                    Email = "customer@bookstore.local",
                    FullName = "Demo Customer",
                    PasswordHash = PasswordHasher.Hash("customer123"),
                    Role = UserRole.Customer
                });

            await db.SaveChangesAsync();
        }

        if (!await db.Categories.AnyAsync())
        {
            db.Categories.AddRange(
                new Category { Name = "Programming", Slug = "programming", Description = "Books about software development and coding." },
                new Category { Name = "Business", Slug = "business", Description = "Books about business, startups, and leadership." },
                new Category { Name = "Self Improvement", Slug = "self-improvement", Description = "Books for personal growth." });
            await db.SaveChangesAsync();
        }

        if (!await db.Authors.AnyAsync())
        {
            db.Authors.AddRange(
                new Author { Name = "Robert C. Martin", Biography = "Software engineering author." },
                new Author { Name = "Andrew Hunt", Biography = "Programming author." },
                new Author { Name = "James Clear", Biography = "Author focused on habits and improvement." });
            await db.SaveChangesAsync();
        }

        if (!await db.Books.AnyAsync())
        {
            var programming = await db.Categories.FirstAsync(c => c.Slug == "programming");
            var self = await db.Categories.FirstAsync(c => c.Slug == "self-improvement");
            var martin = await db.Authors.FirstAsync(a => a.Name == "Robert C. Martin");
            var hunt = await db.Authors.FirstAsync(a => a.Name == "Andrew Hunt");
            var clear = await db.Authors.FirstAsync(a => a.Name == "James Clear");

            db.Books.AddRange(
                new Book { Title = "Clean Code", Isbn = "9780132350884", Description = "A handbook of agile software craftsmanship.", Price = 799, StockQuantity = 25, AuthorId = martin.Id, CategoryId = programming.Id },
                new Book { Title = "The Pragmatic Programmer", Isbn = "9780201616224", Description = "A classic book for professional programmers.", Price = 899, StockQuantity = 18, AuthorId = hunt.Id, CategoryId = programming.Id },
                new Book { Title = "Atomic Habits", Isbn = "9780735211292", Description = "A practical guide to building better habits.", Price = 499, StockQuantity = 8, AuthorId = clear.Id, CategoryId = self.Id });
            await db.SaveChangesAsync();
        }

        if (!await db.Coupons.AnyAsync())
        {
            db.Coupons.AddRange(
                new Coupon { Code = "WELCOME10", Description = "10% off for new customers", Type = CouponType.Percentage, Value = 10, MinimumOrderAmount = 300, UsageLimit = 100, ExpiresAtUtc = DateTime.UtcNow.AddMonths(6), IsActive = true },
                new Coupon { Code = "BOOK50", Description = "Flat 50 off", Type = CouponType.FixedAmount, Value = 50, MinimumOrderAmount = 500, UsageLimit = 50, ExpiresAtUtc = DateTime.UtcNow.AddMonths(3), IsActive = true });
            await db.SaveChangesAsync();
        }

        var customer = await db.Users.FirstOrDefaultAsync(x => x.UserName == "customer");
        if (customer is not null && !await db.CustomerAddresses.AnyAsync(x => x.AppUserId == customer.Id))
        {
            db.CustomerAddresses.Add(new CustomerAddress
            {
                AppUserId = customer.Id,
                Type = AddressType.Home,
                FullName = "Demo Customer",
                PhoneNumber = "9999999999",
                Line1 = "123 Demo Street",
                Line2 = "Near Book Market",
                City = "Bhopal",
                State = "Madhya Pradesh",
                PostalCode = "462001",
                Country = "India",
                IsDefault = true
            });
            await db.SaveChangesAsync();
        }
    }
}
