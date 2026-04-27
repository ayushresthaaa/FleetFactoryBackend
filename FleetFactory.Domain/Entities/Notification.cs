using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Domain.Entities
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // FK to  ApplicationUser.Id, nullable means global/admin broadcast
        public string? UserId { get; set; }

        [Required, MaxLength(50)]
        public string Type { get; set; } = null!; 
        // examples: "low_stock", "overdue_credit", "invoice_sent"

        [Required, MaxLength(150)]
        public string Title { get; set; } = null!;

        public string? Message { get; set; }

        // PartId, InvoiceId, CustomerId, etc.
        public Guid? ReferenceId { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}