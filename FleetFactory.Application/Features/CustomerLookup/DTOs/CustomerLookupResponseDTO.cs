using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Application.Features.CustomerLookup.DTOs
{
    public class CustomerLookupResponseDto
    {
        public Guid CustomerId { get; set; }

        public string UserId { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public List<CustomerLookupVehicleResponseDto> Vehicles { get; set; } = new();
    }

    public class CustomerLookupVehicleResponseDto
    {
        public Guid Id { get; set; }

        public string VehicleNumber { get; set; } = null!;

        public string? Make { get; set; }

        public string? Model { get; set; }

        public int? Year { get; set; }
    }
}