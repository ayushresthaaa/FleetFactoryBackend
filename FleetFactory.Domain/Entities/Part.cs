using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Domain.Entities
{
    public class Part
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // FK to Category
        public Guid? CategoryId { get; set; }

        // FK to Vendor
        public Guid? VendorId { get; set; }

        [Required, MaxLength(50)]
        public string Sku { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal CostPrice { get; set; }

        public int StockQty { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        //image with cloudinary, store the url and public id for deletion
        public string? ImageUrl { get; set; }
        public string? ImagePublicId { get; set; }
        // Navigation
        public PartCategory? Category { get; set; }
        public Vendor? Vendor { get; set; }

        public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
        public ICollection<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; } = new List<PurchaseInvoiceItem>();
        public ICollection<SalesInvoiceItem> SalesInvoiceItems { get; set; } = new List<SalesInvoiceItem>();
    }
}