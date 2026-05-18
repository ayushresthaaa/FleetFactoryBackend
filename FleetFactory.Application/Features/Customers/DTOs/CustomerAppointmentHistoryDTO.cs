using FleetFactory.Domain.Enums;

namespace FleetFactory.Application.Features.Customers.DTOs
{
    public class CustomerAppointmentHistoryStaffSideDTO
    {
        public Guid AppointmentId { get; set; }

        public string? VehicleNumber { get; set; }

        public DateTime ScheduledAt { get; set; }

        public AppointmentStatus Status { get; set; }

        public string? Notes { get; set; }
    }
}