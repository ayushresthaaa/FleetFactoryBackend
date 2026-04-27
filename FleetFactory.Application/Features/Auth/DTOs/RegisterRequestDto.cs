namespace FleetFactory.Application.Features.Auth.DTOs
{
    public class RegisterRequestDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "Customer"; // Default to Customer
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}