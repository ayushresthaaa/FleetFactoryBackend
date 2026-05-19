namespace FleetFactory.Application.Features.PartRequests.DTOs
{
    public class CreatePartRequestRequestDTO
    {
        public Guid? VehicleId { get; set; }

        public string PartName { get; set; } = null!;

        public string? Description { get; set; }
    }
}