using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Application.Features.PurchaseInvoices.DTOs
{
    public class CreatePurchaseInvoiceItemDto
    {
        [Required]
        public Guid PartId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitCost { get; set; }
    }
}