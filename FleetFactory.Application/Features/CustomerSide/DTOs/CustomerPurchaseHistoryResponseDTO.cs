using FleetFactory.Domain.Enums;

namespace FleetFactory.Application.Features.CustomerSide.DTOs
{
    public class CustomerPurchaseHistoryResponseDto
    {
        public Guid SalesInvoiceId { get; set; }

        public string InvoiceNo { get; set; } = null!;

        public InvoiceStatus Status { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? VehicleNumber { get; set; }

        public int ItemCount { get; set; }
    }
}