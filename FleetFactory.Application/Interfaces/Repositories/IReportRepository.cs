using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface IReportRepository
    {
        Task<List<SalesInvoice>> GetSalesInvoicesByDateRangeAsync(DateTime fromDate, DateTime toDate);

        Task<List<PurchaseInvoice>> GetPurchaseInvoicesByDateRangeAsync(DateTime fromDate, DateTime toDate);

        Task<List<CustomerProfile>> GetOverdueCreditCustomersAsync(DateTime thresholdDate);

        Task<List<CustomerProfile>> GetCustomersWithCreditAsync();

        Task<List<SalesInvoiceItem>> GetSalesInvoiceItemsByDateRangeAsync(DateTime fromDate, DateTime toDate);

        Task<List<SalesInvoice>> GetSalesInvoicesWithCustomersAsync(
            DateTime fromDate,
            DateTime toDate);

        Task<List<CustomerProfile>> GetCustomersWithInvoicesAsync();
    }
}