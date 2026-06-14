using OnlineBookstore.Api.Services;

namespace OnlineBookstore.Api.Background;

public class EmailOutboxWorker(IServiceProvider serviceProvider, ILogger<EmailOutboxWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IEmailOutboxService>();
                await service.ProcessPendingAsync(10, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Email outbox worker failed.");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
