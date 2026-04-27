using Microsoft.AspNetCore.Identity; 

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Seeders
{
    public static class RoleSeeder
    {
         public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "Manager", "Staff" };

            foreach(var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                   var result =  await roleManager.CreateAsync(new IdentityRole(roleName));

                    if (!result.Succeeded)
                    {
                        throw new Exception($"Failed to make role {roleName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }
        }
}