using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Infrastructure.Repositories;
using FleetFactory.Infrastructure.Services;
using FleetFactory.Application.Features.Staff.Services;

//import custom services and repositories for DI registration
using FleetFactory.Application.Features.Parts.Services; 

namespace FleetFactory.API.Extensions
{
    public static class ServiceRepositoryExtensions
    {
        public static IServiceCollection AddProjectServicesAndRepositories(this IServiceCollection services)
        {
            //services 
            services.AddScoped<JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICacheService, CacheService>();

            //business logic services
            services.AddScoped<IPartService, PartService>();

            //Admin manages staff 
            services.AddScoped<IStaffService, StaffService>();
            
            //repositories
            services.AddScoped<IPartRepository, PartRepository>();

            return services;
        }
    }
}