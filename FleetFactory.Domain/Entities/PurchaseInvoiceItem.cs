using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Domain.Entities
{
    public class PurchaseInvoiceItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // FK to PurchaseInvoice
        [Required]
        public Guid PurchaseInvoiceId { get; set; }

        // FK to Part
        [Required]
        public Guid PartId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitCost { get; set; }

        // Optional fields for the real world mismatch for delivery
        public int? ReceivedQuantity { get; set; }
        public decimal? ActualUnitCost { get; set; }

        // Navigation
        public PurchaseInvoice PurchaseInvoice { get; set; } = null!;
        public Part Part { get; set; } = null!;
    }
}