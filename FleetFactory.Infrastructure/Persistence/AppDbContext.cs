using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FleetFactory.Infrastructure.Identity;

namespace FleetFactory.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        //identity tables 

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Additional configuration can be added here if needed

            //mapping the app user to the user table 
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users"); 
            });
         }
    }
}