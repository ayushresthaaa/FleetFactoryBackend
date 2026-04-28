namespace FleetFactory.Application.Features.PurchaseInvoices.DTOs
{
    public class PurchaseInvoiceItemResponseDto
    {
        public Guid Id { get; set; }

        public Guid PartId { get; set; }

        public string PartName { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal UnitCost { get; set; }

        public decimal Subtotal { get; set; } // Quantity * UnitCost
    }
}