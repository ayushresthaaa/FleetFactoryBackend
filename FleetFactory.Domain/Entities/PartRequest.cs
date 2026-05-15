using System.ComponentModel.DataAnnotations;
using FleetFactory.Domain.Enums;

namespace FleetFactory.Domain.Entities
{
    public class PartRequest
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CustomerId { get; set; }

        public Guid? VehicleId { get; set; }

        [Required, MaxLength(100)]
        public string PartName { get; set; } = null!;

        public string? Description { get; set; }

        public PartRequestStatus Status { get; set; } = PartRequestStatus.Pending;

        public string? AdminNote { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public CustomerProfile Customer { get; set; } = null!;
        public Vehicle? Vehicle { get; set; }
    }
}