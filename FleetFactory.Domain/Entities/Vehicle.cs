using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Domain.Entities
{
    public class Vehicle
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // FK to CustomerProfile
        [Required]
        public Guid CustomerId { get; set; }

        [Required, MaxLength(30)]
        public string VehicleNumber { get; set; } = null!;

        [MaxLength(50)]
        public string? Make { get; set; }

        [MaxLength(50)]
        public string? Model { get; set; }

        public int? Year { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public CustomerProfile Customer { get; set; } = null!;

        public ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<PartRequest> PartRequests { get; set; } = new List<PartRequest>();
    }
}