using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface ISalesInvoiceRepository
    {
        Task<(List<SalesInvoice> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<SalesInvoice?> GetByIdAsync(Guid id);
        Task<bool> ExistsByInvoiceNoAsync(string invoiceNo);

        Task AddAsync(SalesInvoice invoice);
        void Update(SalesInvoice invoice);
        Task SaveChangesAsync();
    }
}