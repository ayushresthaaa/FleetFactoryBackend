namespace FleetFactory.Application.Features.Customers.DTOs
{
    public class VehicleResponseDto
    {
        public Guid Id { get; set; }

        public string VehicleNumber { get; set; } = null!;

        public string? Make { get; set; }

        public string? Model { get; set; }

        public int? Year { get; set; }
    }
}