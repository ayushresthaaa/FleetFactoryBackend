using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Application.Features.Vendors.DTOs
{
    public class CreateVendorRequestDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(100)]
        public string? ContactName { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        public string? Address { get; set; }
    }
}