namespace FleetFactory.Application.Features.Account.DTOs
{
    public class ChangePasswordRequestDTO
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}