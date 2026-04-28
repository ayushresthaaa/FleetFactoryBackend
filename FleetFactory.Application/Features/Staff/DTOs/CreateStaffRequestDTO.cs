using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Application.Features.Staff.DTOs
{
    public class CreateStaffRequestDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;

        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [MaxLength(20)]
        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateTime? HiredAt { get; set; }
    }
}