namespace FleetFactory.Application.Features.OverdueCredits.DTOs
{
    public class OverdueCreditCustomerDTO
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public decimal CreditBalance { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}