using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface ISalesEmailRepository
    {
        Task<SalesInvoice?> GetInvoiceWithCustomerAsync(Guid salesInvoiceId);
    }
}