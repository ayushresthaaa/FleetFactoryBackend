using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Domain.Entities
{
    public class SalesInvoiceItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // FK to SalesInvoice
        [Required]
        public Guid SalesInvoiceId { get; set; }

        // FK to Part
        [Required]
        public Guid PartId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        // Optional computed to get total price for this line item but not in DB
        public decimal Subtotal => Quantity * UnitPrice;

        // Navigation
        public SalesInvoice SalesInvoice { get; set; } = null!;
        public Part Part { get; set; } = null!;
    }
}