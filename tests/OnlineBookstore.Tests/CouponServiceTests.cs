using Microsoft.EntityFrameworkCore;
using OnlineBookstore.Api.Data;
using OnlineBookstore.Api.DTOs;
using OnlineBookstore.Api.Enums;
using OnlineBookstore.Api.Services;
using Xunit;

namespace OnlineBookstore.Tests;

public class CouponServiceTests
{
    [Fact]
    public async Task Preview_Should_Calculate_Percentage_Discount()
    {
        await using var db = CreateDb();
        var service = new CouponService(db);
        await service.CreateAsync(new CreateCouponRequest("SAVE10", "Save 10 percent", CouponType.Percentage, 10, 100, 5, DateTime.UtcNow.AddDays(1)));

        var preview = await service.PreviewAsync("SAVE10", 1000, 1);

        Assert.True(preview.IsValid);
        Assert.Equal(100, preview.DiscountAmount);
        Assert.Equal(900, preview.TotalAfterDiscount);
    }

    private static AppDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }
}
