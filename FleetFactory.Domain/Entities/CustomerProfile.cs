using System.ComponentModel.DataAnnotations;
using FleetFactory.Infrastructure.Identity;
namespace FleetFactory.Domain.Entities
{
    public class CustomerProfile
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // FK to Identity User
        [Required]
        public string UserId { get; set; } = null!;

        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [MaxLength(20)]
        public string? Phone { get; set; }

        public string? Address { get; set; }

        // Business logic field (important for credit and loyalty later)
        public decimal CreditBalance { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
        // Navigation
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        
        //this is for the sales invoice by the customer
        public ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<PartRequest> PartRequests { get; set; } = new List<PartRequest>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        
        //navigation to the identity user 
        public ApplicationUser User { get; set; } = null!;
    }
}