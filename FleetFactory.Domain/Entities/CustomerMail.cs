using System.ComponentModel.DataAnnotations;

namespace  FleetFactory.Domain.Entities
{
    public class CustomerMail
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Navigation
        public ICollection<SalesInvoice> SalesInvoices { get; set; } = new List<SalesInvoice>();
    }
}