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
            _logger.LogInformation("Low stock background service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var lowStockService = scope.ServiceProvider
                        .GetRequiredService<ILowStockService>();

                    var result = await lowStockService.CheckLowStockAsync();

                    if (result.Success && result.Data?.Any() == true)
                    {
                        _logger.LogWarning(
                            "{Count} low stock part(s) found and notification(s) created.",
                            result.Data.Count
                        );
                    }
                    else
                    {
                        _logger.LogInformation("No low stock parts found.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while checking low stock in background.");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }

            _logger.LogInformation("Low stock background service stopped.");
        }
    }
}