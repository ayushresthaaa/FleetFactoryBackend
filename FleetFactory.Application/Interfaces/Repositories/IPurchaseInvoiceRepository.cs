using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface IPurchaseInvoiceRepository
    {
        Task<(List<PurchaseInvoice> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);

        Task<PurchaseInvoice?> GetByIdAsync(Guid id);

        Task<bool> ExistsByInvoiceNoAsync(string invoiceNo);

        Task AddAsync(PurchaseInvoice invoice);

        void Update(PurchaseInvoice invoice);
    
        Task SaveChangesAsync();
    }
}