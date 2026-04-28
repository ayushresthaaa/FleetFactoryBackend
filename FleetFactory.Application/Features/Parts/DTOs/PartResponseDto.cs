namespace FleetFactory.Application.Features.Parts.DTOs
{
    public class PartResponseDto
    {
        public Guid Id { get; set; }

        public string Sku { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal CostPrice { get; set; }

        public int StockQty { get; set; }

        public bool IsActive { get; set; }

        public Guid? CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public Guid? VendorId { get; set; }
        public string? VendorName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}