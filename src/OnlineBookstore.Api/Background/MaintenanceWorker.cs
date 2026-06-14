using Microsoft.EntityFrameworkCore;
using OnlineBookstore.Api.Data;

namespace OnlineBookstore.Api.Background;

public class MaintenanceWorker(IServiceProvider serviceProvider, ILogger<MaintenanceWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var expiredTokens = await db.RefreshTokens
                    .Where(x => x.ExpiresAtUtc < DateTime.UtcNow.AddDays(-7))
                    .ToListAsync(stoppingToken);
                if (expiredTokens.Count > 0)
                {
                    db.RefreshTokens.RemoveRange(expiredTokens);
                    await db.SaveChangesAsync(stoppingToken);
                    logger.LogInformation("Removed {Count} old refresh tokens.", expiredTokens.Count);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Maintenance worker failed.");
            }

            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }
}
