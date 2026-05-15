using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Application.Features.Customers.DTOs
{
    public class AddVehicleRequestDto
    {
        [Required, MaxLength(30)]
        public string VehicleNumber { get; set; } = null!;

        [MaxLength(50)]
        public string? Make { get; set; }

        [MaxLength(50)]
        public string? Model { get; set; }

        public int? Year { get; set; }
    }
}