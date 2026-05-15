namespace FleetFactory.Application.Features.SalesInvoices.DTOs
{
    public class SalesInvoiceItemResponseDto
    {
        public Guid Id { get; set; }
        public Guid PartId { get; set; }
        public string PartName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}