using FleetFactory.Application.Features.Reports.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Shared.Results;
using FleetFactory.Domain.Entities; 
using FleetFactory.Infrastructure.Helpers;

namespace FleetFactory.Application.Features.Reports.Services
{
    public class ReportService(IReportRepository _reportRepository) : IReportService
    {
        public async Task<ApiResponse<FinancialReportResponseDTO>> GetFinancialReportAsync(string type, DateTime? fromDate = null,
        DateTime? toDate = null)
        {
        var now = DateTime.UtcNow;
        DateTime start;
        DateTime end = toDate ?? now;

        switch (type.ToLower())
        {
            case "daily":
                start = fromDate ?? now.Date;
                break;

            case "monthly":
                start = fromDate ?? new DateTime(now.Year, now.Month, 1);
                break;

            case "yearly":
                start = fromDate ?? new DateTime(now.Year, 1, 1);
                break;

            default:
                start = fromDate ?? now.Date;
                break;
        }

            // var salesInvoices = await _reportRepository.GetSalesInvoicesByDateRangeAsync(fromDate, now);
            // var purchaseInvoices = await _reportRepository.GetPurchaseInvoicesByDateRangeAsync(fromDate, now); 
            var salesInvoices = await _reportRepository.GetSalesInvoicesByDateRangeAsync(start, end);

            var purchaseInvoices = await _reportRepository.GetPurchaseInvoicesByDateRangeAsync(start, end);

            var items = await _reportRepository.GetSalesInvoiceItemsByDateRangeAsync(start, end);

            var grouped = items
                .GroupBy(i => i.Part.Name)
                .Select(g => new
                {
                    Name = g.Key,
                    Quantity = g.Sum(x => x.Quantity)
                })
                .ToList();

            var highest = grouped.OrderByDescending(x => x.Quantity).FirstOrDefault();
            var lowest = grouped.OrderBy(x => x.Quantity).FirstOrDefault();
            
            
            var totalSales = salesInvoices.Sum(s => s.TotalAmount);
            var totalPurchases = purchaseInvoices.Sum(p => p.TotalAmount);

            var response = new FinancialReportResponseDTO
            {
                ReportType = type.ToLower(),
                FromDate = start,
                ToDate = end,
                TotalSales = totalSales,
                TotalPurchases = totalPurchases,
                NetProfit = totalSales - totalPurchases,
                SalesInvoiceCount = salesInvoices.Count,
                PurchaseInvoiceCount = purchaseInvoices.Count,

                HighestSoldItem = highest?.Name,
                HighestSoldQuantity = highest?.Quantity ?? 0,

                LowestSoldItem = lowest?.Name,
                LowestSoldQuantity = lowest?.Quantity ?? 0,
            };

            return ApiResponse<FinancialReportResponseDTO>
                .SuccessResponse(response, "Financial report generated successfully");
        }
        //for overdue credit reports
        public async Task<List<CustomerProfile>> GetUnpaidCreditReportAsync()
        {
            //Calculate 1 month ago using the Nepal timezone helper
            var threshold = DateTimeHelper.NepalNow.AddMonths(-1); 
            
            //Fetch customers with credit balance who haven't paid in 30+ days
            return await _reportRepository.GetOverdueCreditCustomersAsync(threshold); 
        }
        public async Task<List<CustomerProfile>> GetCustomersWithCreditAsync()
        {
            return await _reportRepository.GetCustomersWithCreditAsync();
        }

        public async Task<List<CustomerReportResponseDTO>> GetRegularCustomersReportAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var start = fromDate ?? DateTime.UtcNow.AddMonths(-1);
            var end = toDate ?? DateTime.UtcNow;

            var invoices = await _reportRepository
                .GetSalesInvoicesWithCustomersAsync(start, end);

            var result = invoices
                .GroupBy(i => i.Customer)
                .Select(g => new CustomerReportResponseDTO
                {
                    CustomerId = g.Key.Id,
                    FullName = g.Key.FullName,
                    Phone = g.Key.Phone,
                    CreditBalance = g.Key.CreditBalance,
                    TotalSpent = g.Sum(x => x.TotalAmount),
                    TotalInvoices = g.Count(),
                    LastPurchaseDate = g.Max(x => x.CreatedAt)
                })
                .OrderByDescending(x => x.TotalInvoices)
                .ToList();

            return result;
        }

        public async Task<List<CustomerReportResponseDTO>> GetHighSpendersReportAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var start = fromDate ?? DateTime.UtcNow.AddMonths(-1);
            var end = toDate ?? DateTime.UtcNow;

            var invoices = await _reportRepository
                .GetSalesInvoicesWithCustomersAsync(start, end);

            var result = invoices
                .GroupBy(i => i.Customer)
                .Select(g => new CustomerReportResponseDTO
                {
                    CustomerId = g.Key.Id,
                    FullName = g.Key.FullName,
                    Phone = g.Key.Phone,
                    CreditBalance = g.Key.CreditBalance,
                    TotalSpent = g.Sum(x => x.TotalAmount),
                    TotalInvoices = g.Count(),
                    LastPurchaseDate = g.Max(x => x.CreatedAt)
                })
                .OrderByDescending(x => x.TotalSpent)
                .ToList();

            return result;
        }

        public async Task<List<CustomerReportResponseDTO>> GetPendingCreditCustomersReportAsync(
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            var customers = await _reportRepository
                .GetCustomersWithInvoicesAsync();

            var result = customers
                .Where(c => c.CreditBalance > 0)
                .Select(c => new CustomerReportResponseDTO
                {
                    CustomerId = c.Id,
                    FullName = c.FullName,
                    Phone = c.Phone,
                    CreditBalance = c.CreditBalance,
                    TotalSpent = c.SalesInvoices.Sum(x => x.TotalAmount),
                    TotalInvoices = c.SalesInvoices.Count,
                    LastPurchaseDate = c.SalesInvoices.Any()
                        ? c.SalesInvoices.Max(x => x.CreatedAt)
                        : DateTime.MinValue
                })
                .OrderByDescending(x => x.CreditBalance)
                .ToList();

            return result;
        }
    }
}