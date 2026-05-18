namespace FleetFactory.Application.Features.CustomerProfileManagement.DTOs
{
    public class UpdateMyProfileRequestDto
    {
        public string FullName { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Address { get; set; }
    }
}