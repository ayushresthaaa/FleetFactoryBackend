namespace FleetFactory.Application.Features.CustomerProfileManagement.DTOs
{
    public class AddMyVehicleRequestDto
    {
        public string VehicleNumber { get; set; } = null!;

        public string? Make { get; set; }

        public string? Model { get; set; }

        public int? Year { get; set; }
    }
}