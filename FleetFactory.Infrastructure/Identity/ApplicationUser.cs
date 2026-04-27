using Microsoft.AspNetCore.Identity;
using FleetFactory.Infrastructure.Helpers; 
namespace FleetFactory.Infrastructure.Identity
{
    //the user already has email, username, password hash, security stamp, etc. from IdentityUser
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName  { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTimeHelper.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTimeHelper.UtcNow;

        //add navigation properties for staff and customer profiles
    }
}