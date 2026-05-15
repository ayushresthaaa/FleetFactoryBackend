namespace FleetFactory.Application.Features.SalesInvoices.DTOs
{
    public class CreateSalesInvoiceItemDto
    {
        public Guid PartId { get; set; }
        public int Quantity { get; set; }
    }
}