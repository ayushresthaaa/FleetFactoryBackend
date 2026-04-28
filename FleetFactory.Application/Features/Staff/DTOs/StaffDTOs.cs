using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Application.Features.Staff.DTOs
{
    //REQUEST DTOs 

    public class RegisterStaffRequestDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, MinLength(6)]
        public string Password { get; set; } = null!;

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = null!;

        [MaxLength(20)]
        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateTime? HiredAt { get; set; }

        
        public string Role { get; set; } = "Staff";
    }

    public class UpdateStaffRequestDto
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = null!;

        [MaxLength(20)]
        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateTime? HiredAt { get; set; }

        public string Role { get; set; } = "Staff";
    }

    public class SetStaffStatusRequestDto
    {
        [Required]
        public bool IsActive { get; set; }
    }

    //RESPONSE DTOs

    public class StaffResponseDto
    {
    
        public string UserId { get; set; } = null!;
        public Guid StaffProfileId { get; set; }
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? HiredAt { get; set; }
        public string Role { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class StaffSummaryDto
    {
        public string UserId { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string Role { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime? HiredAt { get; set; }
    }
}
