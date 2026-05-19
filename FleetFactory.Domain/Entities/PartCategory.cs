using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Domain.Entities
{
    public class PartCategory
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        //category-specific low stock threshold
        public int LowStockThreshold { get; set; } = 10;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<Part> Parts { get; set; } = new List<Part>();
    }
}