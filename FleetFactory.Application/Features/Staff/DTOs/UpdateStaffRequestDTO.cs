using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Application.Features.Staff.DTOs
{
    public class UpdateStaffRequestDTO
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [MaxLength(20)]
        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateTime? HiredAt { get; set; }

        public bool IsActive { get; set; } = true;
    }
}