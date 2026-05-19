namespace FleetFactory.Application.Features.Reviews.DTOs
{
    public class ReviewResponseDTO
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public Guid AppointmentId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public bool IsVisible { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}