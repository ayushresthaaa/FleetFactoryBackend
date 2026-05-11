using System.ComponentModel.DataAnnotations;

namespace FleetFactory.Domain.Entities
{
    public class Review
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CustomerId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }

        public bool IsVisible { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public CustomerProfile Customer { get; set; } = null!;
    }
}