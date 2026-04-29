namespace FleetFactory.Application.Features.Reports.DTOs
{
    public class FinancialReportResponseDTO
    {
        public string ReportType { get; set; } = null!;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public decimal TotalSales { get; set; }

        public decimal TotalPurchases { get; set; }

        public decimal NetProfit { get; set; }

        public int SalesInvoiceCount { get; set; }

        public int PurchaseInvoiceCount { get; set; }
    }
}