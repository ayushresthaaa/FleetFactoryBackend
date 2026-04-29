namespace FleetFactory.Application.Features.Customers.DTOs
{
    public class CustomerSearchResponseDto
    {
        public Guid CustomerId { get; set; }

        public string UserId { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public List<VehicleResponseDto> Vehicles { get; set; } = new();
    }
}