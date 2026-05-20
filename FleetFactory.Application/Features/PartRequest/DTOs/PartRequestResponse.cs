namespace FleetFactory.Application.Features.PartRequests.DTOs
{
    public class PartRequestResponseDTO
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public Guid? VehicleId { get; set; }

        public string? VehicleNumber { get; set; }

        public Guid? PartId { get; set; }

        public string PartName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? AdminNote { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}