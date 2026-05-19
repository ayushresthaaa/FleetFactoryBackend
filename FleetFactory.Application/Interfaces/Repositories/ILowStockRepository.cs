using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface ILowStockRepository
    {
        Task<List<Part>> GetLowStockPartsAsync();

        Task AddNotificationAsync(Notification notification);

        Task SaveChangesAsync();
    }
}