using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface ICustomerSideRepository
    {
        Task<CustomerProfile?> GetCustomerByUserIdAsync(string userId);

        Task<List<SalesInvoice>> GetPurchaseHistoryAsync(Guid customerId);

        Task<List<Appointment>> GetAppointmentHistoryAsync(Guid customerId);
    }
}