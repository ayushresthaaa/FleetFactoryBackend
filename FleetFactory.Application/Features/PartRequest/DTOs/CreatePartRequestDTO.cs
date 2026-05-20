namespace FleetFactory.Application.Features.PartRequests.DTOs
{
    public class CreatePartRequestRequestDTO
    {
        public Guid? VehicleId { get; set; }

        // Existing part request
        public Guid? PartId { get; set; }

        // Custom part request
        public string? PartName { get; set; }

        public string? Description { get; set; }
    }
}