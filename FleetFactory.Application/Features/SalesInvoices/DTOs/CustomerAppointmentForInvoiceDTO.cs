namespace FleetFactory.Application.Features.SalesInvoices.DTOs
{
    public class CustomerAppointmentForInvoiceDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? VehicleId { get; set; }
        public string? VehicleNumber { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string Status { get; set; } = "";
        public string? Notes { get; set; }
    }
}