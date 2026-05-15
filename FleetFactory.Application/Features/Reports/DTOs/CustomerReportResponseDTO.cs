namespace FleetFactory.Application.Features.Reports.DTOs
{
    public class CustomerReportResponseDTO
    {
        public Guid CustomerId { get; set; }

        public string FullName { get; set; } = null!;

        public string? Phone { get; set; }

        public decimal CreditBalance { get; set; }

        public decimal TotalSpent { get; set; }

        public int TotalInvoices { get; set; }

        public DateTime LastPurchaseDate { get; set; }
    }
}