using FleetFactory.Application.Interfaces.Services;

namespace FleetFactory.Infrastructure.Services
{
    public class LowStockBackgroundService(
        IServiceScopeFactory _scopeFactory,
        ILogger<LowStockBackgroundService> _logger
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var lowStockService = scope.ServiceProvider
                        .GetRequiredService<ILowStockService>();

                    await lowStockService.CheckLowStockAsync(10);

                    _logger.LogInformation("Low stock background check completed.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while checking low stock in background.");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}