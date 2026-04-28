namespace FleetFactory.Application.Features.Vendors.DTOs
{
    public class VendorResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? ContactName { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}