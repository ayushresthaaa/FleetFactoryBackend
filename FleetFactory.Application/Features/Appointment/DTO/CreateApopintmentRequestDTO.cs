namespace FleetFactory.Application.Features.Appointments.DTOs
{
    public class CreateMyAppointmentRequestDTO
    {
        public Guid? VehicleId { get; set; }

        public DateTime ScheduledAt { get; set; }

        public string? Notes { get; set; }
    }
}