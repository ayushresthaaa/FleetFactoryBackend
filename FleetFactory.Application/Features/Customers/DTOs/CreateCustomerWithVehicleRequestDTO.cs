using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Application.Features.Customers.DTOs
{
    public class CreateCustomerWithVehicleRequestDto
    {
        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [MaxLength(20)]
        public string? Phone { get; set; }

        public string? Address { get; set; }

        [Required, MaxLength(30)]
        public string VehicleNumber { get; set; } = null!;

        [MaxLength(50)]
        public string? Make { get; set; }

        [MaxLength(50)]
        public string? Model { get; set; }

        public int? Year { get; set; }
    }
}