using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Infrastructure.Repositories;
using FleetFactory.Infrastructure.Services;

//import custom services and repositories for DI registration
using FleetFactory.Application.Features.Parts.Services; 
using FleetFactory.Application.Features.PurchaseInvoices.Services;
using FleetFactory.Application.Features.Vendors.Services;

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
            services.AddScoped<IPurchaseInvoiceService, PurchaseInvoiceService>();
            services.AddScoped<IVendorService, VendorService>();
            //repositories
            services.AddScoped<IPartRepository, PartRepository>();
            services.AddScoped<IPurchaseInvoiceRepository, PurchaseInvoiceRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            return services;
        }
    }
}