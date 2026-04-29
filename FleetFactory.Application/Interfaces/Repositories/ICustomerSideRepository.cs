using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface ICustomerSideRepository
    {
        Task<List<SalesInvoice>> GetPurchaseHistoryAsync(Guid customerId);
    }
}