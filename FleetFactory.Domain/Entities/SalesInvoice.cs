using System.ComponentModel.DataAnnotations;
using FleetFactory.Domain.Enums;

namespace FleetFactory.Domain.Entities
{
    public class SalesInvoice
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // FK to CustomerProfile
        [Required]
        public Guid CustomerId { get; set; }

        // FK to Vehicle, optional
        public Guid? VehicleId { get; set; }

        [Required, MaxLength(50)]
        public string InvoiceNo { get; set; } = null!;

        public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;

        public PaymentMethod? PaymentMethod { get; set; }

        public decimal Subtotal { get; set; } = 0;

        public decimal DiscountPct { get; set; } = 0;

        [MaxLength(100)]
        public string? DiscountReason { get; set; }

        public decimal TotalAmount { get; set; } = 0;

        public DateTime? DueDate { get; set; }
        public DateTime? PaidAt { get; set; }

        // FK to ApplicationUser.Id, no navigation for now
        [Required]
        public string CreatedById { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Guid? AppointmentId { get; set; }

        public decimal ServiceCharge { get; set; } = 0;

        public string? ServiceDescription { get; set; }
        // Navigation
        public CustomerProfile Customer { get; set; } = null!;

        public Vehicle? Vehicle { get; set; }

        public Appointment? Appointment { get; set; }

        public ICollection<SalesInvoiceItem> Items { get; set; } = new List<SalesInvoiceItem>();
    }
}