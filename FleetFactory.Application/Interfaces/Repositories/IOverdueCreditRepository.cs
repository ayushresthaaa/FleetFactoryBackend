using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface IOverdueCreditRepository
    {
        Task<List<CustomerProfile>> GetOverdueCreditCustomersAsync(DateTime thresholdDate);

        Task<CustomerProfile?> GetOverdueCreditCustomerByIdAsync(
            Guid customerId,
            DateTime thresholdDate
        );

        Task AddNotificationAsync(Notification notification);

        Task SaveChangesAsync();
    }
}