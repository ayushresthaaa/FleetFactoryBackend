using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Application.Features.Parts.DTOs
{
    public class UpdatePartRequestDto
    {
        public Guid? CategoryId { get; set; }

        public Guid? VendorId { get; set; }

        [Required, MaxLength(50)]
        public string Sku { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal CostPrice { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int StockQty { get; set; }

        public bool IsActive { get; set; }
    }
}