using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Application.Features.PurchaseInvoices.DTOs
{
    public class CreatePurchaseInvoiceRequestDto
    {
        [Required]
        public Guid VendorId { get; set; }

        [Required, MaxLength(50)]
        public string InvoiceNo { get; set; } = null!;

        [Required]
        public List<CreatePurchaseInvoiceItemDto> Items { get; set; } = new();
    }
}