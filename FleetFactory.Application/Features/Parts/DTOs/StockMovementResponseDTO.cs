using FleetFactory.Domain.Enums;

namespace FleetFactory.Application.Features.Parts.DTOs
{
    public class StockMovementResponseDto
    {
        public Guid Id { get; set; }

        public Guid PartId { get; set; }

        public string? PartName { get; set; }

        public StockMovementType MovementType { get; set; }

        public int Quantity { get; set; }

        public Guid? ReferenceId { get; set; }

        public string? Note { get; set; }

        public string? CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}