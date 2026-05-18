namespace FleetFactory.Application.Features.CustomerProfileManagement.DTOs
{
    public class UpdateMyVehicleRequestDto
    {
        public string VehicleNumber { get; set; } = null!;

        public string? Make { get; set; }

        public string? Model { get; set; }

        public int? Year { get; set; }
    }
}