namespace  FleetFactory.Domain.Entities
{
    public class SendSalesInvoiceMailDto
    {
        public Guid CustomerId { get; set; }
        public Guid SalesInvoiceId { get; set; }
    }
}