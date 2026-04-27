using Microsoft.AspNetCore.Identity;
using FleetFactory.Infrastructure.Helpers; 
using FleetFactory.Domain.Entities; // for navigation properties
namespace FleetFactory.Infrastructure.Identity
{
    //the user already has email, username, password hash, security stamp, etc. from IdentityUser
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName  { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTimeHelper.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTimeHelper.UtcNow;

        //add navigation properties for staff and customer profiles
        public StaffProfile? StaffProfile { get; set; }
        public CustomerProfile? CustomerProfile { get; set; }

        //created records 
        public ICollection<PurchaseInvoice> CreatedPurchaseInvoices { get; set; } = new List<PurchaseInvoice>();

        //this is for the staff/admin
        public ICollection<SalesInvoice> CreatedSalesInvoices { get; set; } = new List<SalesInvoice>();
        public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}