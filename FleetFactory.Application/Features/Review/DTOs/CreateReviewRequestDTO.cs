namespace FleetFactory.Application.Features.Reviews.DTOs
{
    public class CreateReviewRequestDTO
    {
        public Guid AppointmentId { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }
    }
}