namespace FleetFactory.Application.Features.CustomerSide.DTOs
{
    public class CustomerAppointmentHistoryResponseDto
    {
        public Guid AppointmentId { get; set; }

        public string? VehicleNumber { get; set; }

        public DateTime ScheduledAt { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}