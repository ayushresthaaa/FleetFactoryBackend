namespace FleetFactory.Application.Features.Reports.DTOs
{
    public class PendingCreditDTO
    {
        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; } = null!;

        public string? Phone { get; set; }

        public decimal CreditBalance { get; set; }
    }
}