using System.ComponentModel.DataAnnotations;
using FleetFactory.Domain.Enums;

namespace FleetFactory.Domain.Entities
{
    public class Appointment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CustomerId { get; set; }

        public Guid? VehicleId { get; set; }

        [Required]
        public DateTime ScheduledAt { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public CustomerProfile Customer { get; set; } = null!;
        public Vehicle? Vehicle { get; set; }

        public ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();
    }
}