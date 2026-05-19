using FleetFactory.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FleetFactory.Infrastructure.Services
{
    public class OverdueCreditBackgroundService(
        IServiceScopeFactory _scopeFactory
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken
        )
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var overdueCreditService = scope.ServiceProvider
                    .GetRequiredService<IOverdueCreditService>();

                try
                {
                    Console.WriteLine(
                        "Running overdue credit reminder check..."
                    );

                    await overdueCreditService
                        .SendReminderToAllAsync();

                    Console.WriteLine(
                        "Overdue credit reminder check completed."
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Overdue credit background service error: {ex.Message}"
                    );
                }

                //runs every 24 hours
                await Task.Delay(
                    TimeSpan.FromHours(24),
                    stoppingToken
                );
            }
        }
    }
}