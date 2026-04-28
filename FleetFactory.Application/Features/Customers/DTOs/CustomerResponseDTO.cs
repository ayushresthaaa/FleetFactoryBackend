namespace FleetFactory.Application.Features.Customers.DTOs
{
    public class CustomerResponseDto
    {
        public Guid Id { get; set; }

        public string UserId { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public decimal CreditBalance { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<VehicleResponseDto> Vehicles { get; set; } = new();
    }
}