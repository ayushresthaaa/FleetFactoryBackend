using FleetFactory.Domain.Enums;

namespace FleetFactory.Application.Features.SalesInvoices.DTOs
{
    public class CreateSalesInvoiceRequestDto
    {
        public Guid CustomerId { get; set; }
        public Guid? VehicleId { get; set; }
        public Guid? AppointmentId { get; set; }

        public string InvoiceNo { get; set; } = null!;

        public PaymentMethod? PaymentMethod { get; set; }

        public decimal ServiceCharge { get; set; } = 0;
        public string? ServiceDescription { get; set; }

        public DateTime? DueDate { get; set; }

        public List<CreateSalesInvoiceItemDto> Items { get; set; } = new();
    }
}