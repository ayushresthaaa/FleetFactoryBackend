using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task CreateAsync(
            string? userId,
            string type,
            string title,
            string message,
            Guid? referenceId = null
        );
    }
}