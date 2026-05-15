namespace FleetFactory.Application.Features.SalesInvoices.DTOs
{
    public class SalesInvoiceResponseDto
    {
        public Guid Id { get; set; }
        public string InvoiceNo { get; set; } = "";

        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = "";

        public Guid? VehicleId { get; set; }
        public string? VehicleNumber { get; set; }

        public Guid? AppointmentId { get; set; }

        public string Status { get; set; } = "";
        public string? PaymentMethod { get; set; }

        public decimal ServiceCharge { get; set; }
        public string? ServiceDescription { get; set; }

        public decimal Subtotal { get; set; }
        public decimal DiscountPct { get; set; }
        public string? DiscountReason { get; set; }
        public decimal TotalAmount { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<SalesInvoiceItemResponseDto> Items { get; set; } = new();
    }
}