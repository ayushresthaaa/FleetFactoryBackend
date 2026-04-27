using System.ComponentModel.DataAnnotations;
using FleetFactory.Domain.Enums;

namespace FleetFactory.Domain.Entities
{
    public class PurchaseInvoice
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // FK toVendor
        [Required]
        public Guid VendorId { get; set; }

        [Required, MaxLength(50)]
        public string InvoiceNo { get; set; } = null!;

        public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;

        public decimal TotalAmount { get; set; } = 0;

        public DateTime? PaidAt { get; set; }

        // FK to ApplicationUser.Id, no navigation for now
        [Required]
        public string CreatedById { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Vendor Vendor { get; set; } = null!;
        public ICollection<PurchaseInvoiceItem> Items { get; set; } = new List<PurchaseInvoiceItem>();
    }
}