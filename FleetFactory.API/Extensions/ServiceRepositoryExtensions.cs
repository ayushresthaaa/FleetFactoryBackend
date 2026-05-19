using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;


using FleetFactory.Infrastructure.Repositories;
using FleetFactory.Infrastructure.Services;

//import custom services and repositories for DI registration
using FleetFactory.Application.Features.Parts.Services; 
using FleetFactory.Application.Features.PurchaseInvoices.Services;
using FleetFactory.Application.Features.Vendors.Services;
using FleetFactory.Application.Features.Customers.Services;
using FleetFactory.Application.Features.Staff.Services;
using FleetFactory.Application.Features.CustomerSide.Services;
using FleetFactory.Application.Features.LowStock.Services;
using FleetFactory.Application.Features.Reports.Services;
using FleetFactory.Application.Features.SalesInvoices.Services;
using FleetFactory.Application.Features.PartCategories.Services;
using FleetFactory.Application.Features.CustomerLookup.Services; 
using FleetFactory.Application.Features.CustomerProfileManagement.Services;
using FleetFactory.Application.Features.Appointments.Services;
using FleetFactory.Application.Features.PartRequests.Services;
using FleetFactory.Application.Features.Reviews.Services;
using FleetFactory.Application.Features.OverdueCredits.Services;
using FleetFactory.Infrastructure.Services;

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
            services.AddScoped<IEmailService, EmailService>();

            //business logic services
            services.AddScoped<IPartService, PartService>();
            services.AddScoped<IPurchaseInvoiceService, PurchaseInvoiceService>();
            services.AddScoped<ISalesInvoiceService, SalesInvoiceService>();
            services.AddScoped<IImageService, CloudinaryImageService>();
            services.AddScoped<IVendorService, VendorService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<ICustomerSideService, CustomerSideService>();
            services.AddScoped<ILowStockService, LowStockService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IPartCategoryService, PartCategoryService>();
            services.AddScoped<ICustomerLookupService, CustomerLookupService>();
            services.AddScoped<ISendInvoiceEmailService, SendInvoiceEmailService>();
            services.AddScoped<ICustomerProfileService, CustomerProfileService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IPartRequestService, PartRequestService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IOverdueCreditService, OverdueCreditService>();
            services.AddScoped<INotificationService, NotificationService>();
            //repositories
            services.AddScoped<IPartRepository, PartRepository>();
            services.AddScoped<IPurchaseInvoiceRepository, PurchaseInvoiceRepository>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<ICustomerSideRepository, CustomerSideRepository>();
            services.AddScoped<ILowStockRepository, LowStockRepository>();
            services.AddScoped<ISalesInvoiceRepository, SalesInvoiceRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IPartCategoryRepository, PartCategoryRepository>();
            services.AddScoped<ICustomerLookupRepository, CustomerLookupRepository>();
            services.AddScoped<ICustomerProfileRepository, CustomerProfileRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IPartRequestRepository, PartRequestRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IOverdueCreditRepository, OverdueCreditRepository>();
            return services;
        }
    }
}