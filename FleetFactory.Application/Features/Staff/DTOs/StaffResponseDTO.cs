namespace FleetFactory.Application.Features.Staff.DTOs
{
    public class StaffResponseDTO
    {
        public Guid StaffProfileId { get; set; }

        public string UserId { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateTime? HiredAt { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}