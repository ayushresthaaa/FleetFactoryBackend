using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface IReportRepository
    {
        Task<List<SalesInvoice>> GetSalesInvoicesByDateRangeAsync(DateTime fromDate, DateTime toDate);

        Task<List<PurchaseInvoice>> GetPurchaseInvoicesByDateRangeAsync(DateTime fromDate, DateTime toDate);
    }
}