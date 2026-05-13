using FleetFactory.Application.Features.Reports.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Shared.Results;
using FleetFactory.Domain.Entities; 
using FleetFactory.Shared.Helpers;

namespace FleetFactory.Application.Features.Reports.Services
{
    public class ReportService(IReportRepository _reportRepository) : IReportService
    {
        public async Task<ApiResponse<FinancialReportResponseDTO>> GetFinancialReportAsync(string type)
        {
            var now = DateTime.UtcNow;
            var today = now.Date;

            DateTime fromDate;

            switch (type.ToLower())
            {
                case "daily":
                    fromDate = today;
                    break;

                case "weekly":
                    fromDate = today.AddDays(-7);
                    break;

                case "monthly":
                    fromDate = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                    break;

                case "yearly":
                    fromDate = new DateTime(today.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    break;

                default:
                    return ApiResponse<FinancialReportResponseDTO>
                        .ErrorResponse("Invalid report type. Use daily, weekly, monthly, or yearly.");
            }

            var salesInvoices = await _reportRepository.GetSalesInvoicesByDateRangeAsync(fromDate, now);
            var purchaseInvoices = await _reportRepository.GetPurchaseInvoicesByDateRangeAsync(fromDate, now);

            var totalSales = salesInvoices.Sum(s => s.TotalAmount);
            var totalPurchases = purchaseInvoices.Sum(p => p.TotalAmount);

            var response = new FinancialReportResponseDTO
            {
                ReportType = type.ToLower(),
                FromDate = fromDate,
                ToDate = now,
                TotalSales = totalSales,
                TotalPurchases = totalPurchases,
                NetProfit = totalSales - totalPurchases,
                SalesInvoiceCount = salesInvoices.Count,
                PurchaseInvoiceCount = purchaseInvoices.Count
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
    }
}