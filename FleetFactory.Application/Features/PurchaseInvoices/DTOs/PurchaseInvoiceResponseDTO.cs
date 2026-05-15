namespace FleetFactory.Application.Features.PurchaseInvoices.DTOs
{
    public class PurchaseInvoiceResponseDto
    {
        public Guid Id { get; set; }

        public string InvoiceNo { get; set; } = null!;

        public Guid VendorId { get; set; }

        public string VendorName { get; set; } = null!;

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public List<PurchaseInvoiceItemResponseDto> Items { get; set; } = new();
    }
}