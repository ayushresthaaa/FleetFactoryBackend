namespace FleetFactory.Application.Features.Appointments.DTOs
{
    public class AppointmentResponseDTO
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public Guid? VehicleId { get; set; }

        public string? VehicleNumber { get; set; }

        public DateTime ScheduledAt { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}