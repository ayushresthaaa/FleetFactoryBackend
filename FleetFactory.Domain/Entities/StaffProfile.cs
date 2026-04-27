using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FleetFactory.Infrastructure.Identity;

namespace FleetFactory.Domain.Entities
{
    public class StaffProfile
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        //fk for application user
        [Required]
        public string UserId { get; set; } = null!;

        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [MaxLength(20)]
        public string? Phone { get; set; }

        public string? Address { get; set; }

        public DateTime? HiredAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;
    }
}