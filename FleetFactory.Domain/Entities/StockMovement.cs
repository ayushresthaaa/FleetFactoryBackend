using System.ComponentModel.DataAnnotations;
using FleetFactory.Domain.Enums;

namespace FleetFactory.Domain.Entities
{
    public class StockMovement
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // FK to Part
        [Required]
        public Guid PartId { get; set; }

        [Required]
        public StockMovementType MovementType { get; set; }

        // Positive for stock in, negative for stock out
        public int Quantity { get; set; }

        // Invoice id or adjustment reference
        public Guid? ReferenceId { get; set; }

        public string? Note { get; set; }

        // FK to ApplicationUser.Id, no navigation for now
        public string? CreatedById { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Part Part { get; set; } = null!;
    }
}