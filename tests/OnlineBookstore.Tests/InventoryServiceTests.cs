using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using OnlineBookstore.Api.Data;
using OnlineBookstore.Api.DTOs;
using OnlineBookstore.Api.Entities;
using OnlineBookstore.Api.Enums;
using OnlineBookstore.Api.Services;
using Xunit;

namespace OnlineBookstore.Tests;

public class InventoryServiceTests
{
    [Fact]
    public async Task AdjustStock_Should_Update_Quantity()
    {
        await using var db = CreateDb();
        db.Books.Add(new Book { Title = "Test Book", Isbn = "123", Price = 100, StockQuantity = 10, AuthorId = 1, CategoryId = 1 });
        await db.SaveChangesAsync();

        var service = new InventoryService(db, new SilentNotificationService());
        await service.AdjustStockAsync(1, -3, "Test", "UNIT");

        var book = await db.Books.FindAsync(1);
        Assert.Equal(7, book!.StockQuantity);
    }

    private static AppDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private sealed class SilentNotificationService : INotificationService
    {
        public Task NotifyAdminsAsync(NotificationType type, string title, string message) => Task.CompletedTask;
        public Task NotifyUserAsync(int userId, NotificationType type, string title, string message) => Task.CompletedTask;
        public Task<IReadOnlyList<NotificationDto>> GetForUserAsync(int userId, bool includeRead) => Task.FromResult<IReadOnlyList<NotificationDto>>(Array.Empty<NotificationDto>());
        public Task MarkReadAsync(int userId, int notificationId) => Task.CompletedTask;
    }
}
