using FleetFactory.Domain.Enums;

namespace FleetFactory.Application.Features.Customers.DTOs
{
    public class CustomerSalesInvoiceHistoryDTO
    {
        public Guid SalesInvoiceId { get; set; }

        public string InvoiceNo { get; set; } = null!;

        public InvoiceStatus Status { get; set; }

        public decimal Subtotal { get; set; }

        public decimal DiscountPct { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}