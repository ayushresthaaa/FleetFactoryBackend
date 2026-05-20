using FleetFactory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace FleetFactory.Infrastructure.Seeders
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var email = "admin@fleetfactory.com";
            var password = "Admin@12345";

            var admin = await userManager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = "System",
                    LastName = "Admin",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(admin, password);

                if (!result.Succeeded)
                {
                    throw new Exception(
                        $"Failed to create admin: {string.Join(", ", result.Errors.Select(e => e.Description))}"
                    );
                }
            }

            if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}