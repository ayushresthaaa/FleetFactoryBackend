using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FleetFactory.Infrastructure.Identity;
using FleetFactory.Domain.Entities; // for DbSet properties
namespace FleetFactory.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        //domain tables 
        // user/profile domain
        public DbSet<CustomerProfile> CustomerProfiles => Set<CustomerProfile>();
        public DbSet<StaffProfile> StaffProfiles => Set<StaffProfile>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();

        // inventory domain
        public DbSet<Vendor> Vendors => Set<Vendor>();
        public DbSet<PartCategory> PartCategories => Set<PartCategory>();
        public DbSet<Part> Parts => Set<Part>();
        public DbSet<StockMovement> StockMovements => Set<StockMovement>();

        // purchase domain
        public DbSet<PurchaseInvoice> PurchaseInvoices => Set<PurchaseInvoice>();
        public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems => Set<PurchaseInvoiceItem>();

        // sales domain
        public DbSet<SalesInvoice> SalesInvoices => Set<SalesInvoice>();
        public DbSet<SalesInvoiceItem> SalesInvoiceItems => Set<SalesInvoiceItem>();

        // notification domain
        public DbSet<Notification> Notifications => Set<Notification>();

        //appointment domain and stuff related to part request and the review
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<PartRequest> PartRequests => Set<PartRequest>();
        public DbSet<Review> Reviews => Set<Review>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Additional configuration can be added here if needed

            //mapping the app user to the user table 
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users"); 
            });

            //fluent API configuration for relationships 
            //user - staffprofile 
            builder.Entity<StaffProfile>()
                .HasOne<ApplicationUser>()
                .WithOne(u => u.StaffProfile)
                .HasForeignKey<StaffProfile>(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            //user - customerprofile
            builder.Entity<CustomerProfile>()
                .HasOne(c => c.User)
                .WithOne(u => u.CustomerProfile)
                .HasForeignKey<CustomerProfile>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
                
            //customerprofile - vehicle
            builder.Entity<Vehicle>()
                .HasOne(v => v.Customer)
                .WithMany(c => c.Vehicles)
                .HasForeignKey(v => v.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            //customerprofile - salesinvoice 
            builder.Entity<SalesInvoice>()
                .HasOne(s => s.Customer)
                .WithMany(c => c.SalesInvoices)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            //vehicle - salesinvoice
             builder.Entity<SalesInvoice>()
                .HasOne(s => s.Vehicle)
                .WithMany(v => v.SalesInvoices)
                .HasForeignKey(s => s.VehicleId)
                .OnDelete(DeleteBehavior.SetNull);

            //vendor - parts 
            builder.Entity<Part>()
                .HasOne(p => p.Vendor)
                .WithMany(v => v.Parts)
                .HasForeignKey(p => p.VendorId)
                .OnDelete(DeleteBehavior.SetNull);
            
            //partcategory - parts
            builder.Entity<Part>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Parts)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
            
            //vendor - purchaseinvoice
            builder.Entity<PurchaseInvoice>()
                .HasOne(p => p.Vendor)
                .WithMany(v => v.PurchaseInvoices)
                .HasForeignKey(p => p.VendorId)
                .OnDelete(DeleteBehavior.Restrict);
            
            //purchaseinvoice - item
            builder.Entity<PurchaseInvoiceItem>()
                .HasOne(i => i.PurchaseInvoice)
                .WithMany(p => p.Items)
                .HasForeignKey(i => i.PurchaseInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            //part - purchaseinvoiceitem
             builder.Entity<PurchaseInvoiceItem>()
                .HasOne(i => i.Part)
                .WithMany(p => p.PurchaseInvoiceItems)
                .HasForeignKey(i => i.PartId)
                .OnDelete(DeleteBehavior.Restrict);

            //salesinvoice - item
            builder.Entity<SalesInvoiceItem>()
                .HasOne(i => i.SalesInvoice)
                .WithMany(s => s.Items)
                .HasForeignKey(i => i.SalesInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
            
            //part - salesinvoiceitem
            builder.Entity<SalesInvoiceItem>()
                .HasOne(i => i.Part)
                .WithMany(p => p.SalesInvoiceItems)
                .HasForeignKey(i => i.PartId)
                .OnDelete(DeleteBehavior.Restrict);
            
            //part - stockmovements 
             builder.Entity<StockMovement>()
                .HasOne(s => s.Part)
                .WithMany(p => p.StockMovements)
                .HasForeignKey(s => s.PartId)
                .OnDelete(DeleteBehavior.Restrict);
            
            //user - created purchaseinvoices
            builder.Entity<PurchaseInvoice>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.CreatedPurchaseInvoices)
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            
            //user - created salesinvoices
             builder.Entity<SalesInvoice>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.CreatedSalesInvoices)
                .HasForeignKey(s => s.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            
            //user - stockmovements
             builder.Entity<StockMovement>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.StockMovements)
                .HasForeignKey(s => s.CreatedById)
                .OnDelete(DeleteBehavior.SetNull);
            
            //user - notifications
             builder.Entity<Notification>()
                .HasOne<ApplicationUser>()
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.SetNull);
            
            //unique indexes 
            //index means 
             builder.Entity<Part>()
                .HasIndex(p => p.Sku)
                .IsUnique();
            
            builder.Entity<PartCategory>()
                .HasIndex(c => c.Name)
                .IsUnique();

            builder.Entity<Vehicle>()
                .HasIndex(v => v.VehicleNumber)
                .IsUnique();

            builder.Entity<PurchaseInvoice>()
                .HasIndex(p => p.InvoiceNo)
                .IsUnique();

            builder.Entity<SalesInvoice>()
                .HasIndex(s => s.InvoiceNo)
                .IsUnique();

            builder.Entity<SalesInvoice>()
                .HasOne(s => s.Appointment)
                .WithMany(a => a.SalesInvoices)
                .HasForeignKey(s => s.AppointmentId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Appointment>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Appointment>()
                .HasOne(a => a.Vehicle)
                .WithMany(v => v.Appointments)
                .HasForeignKey(a => a.VehicleId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<PartRequest>()
                .HasOne(p => p.Customer)
                .WithMany(c => c.PartRequests)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PartRequest>()
                .HasOne(p => p.Vehicle)
                .WithMany(v => v.PartRequests)
                .HasForeignKey(p => p.VehicleId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Review>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            //precisoin required 
            builder.Entity<Part>(entity =>
            {
                entity.Property(p => p.UnitPrice).HasPrecision(12, 2);
                entity.Property(p => p.CostPrice).HasPrecision(12, 2);
            });

            builder.Entity<CustomerProfile>()
                .Property(c => c.CreditBalance)
                .HasPrecision(12, 2);

            builder.Entity<PurchaseInvoice>(entity =>
            {
                entity.Property(p => p.TotalAmount).HasPrecision(12, 2);
            });

            builder.Entity<PurchaseInvoiceItem>(entity =>
            {
                entity.Property(i => i.UnitCost).HasPrecision(12, 2);
                entity.Property(i => i.ActualUnitCost).HasPrecision(12, 2);
            });

            // builder.Entity<SalesInvoice>(entity =>
            // {
            //     entity.Property(s => s.Subtotal).HasPrecision(12, 2);
            //     entity.Property(s => s.DiscountPct).HasPrecision(5, 2);
            //     entity.Property(s => s.TotalAmount).HasPrecision(12, 2);
            // });

            builder.Entity<SalesInvoiceItem>()
                .Property(i => i.UnitPrice)
                .HasPrecision(12, 2);

            builder.Entity<SalesInvoice>(entity =>
                {
                    entity.Property(s => s.Subtotal).HasPrecision(12, 2);
                    entity.Property(s => s.DiscountPct).HasPrecision(5, 2);
                    entity.Property(s => s.TotalAmount).HasPrecision(12, 2);
                    entity.Property(s => s.ServiceCharge).HasPrecision(12, 2);
                });

            //ignoring the computed property for DB
            builder.Entity<SalesInvoiceItem>()
                .Ignore(i => i.Subtotal); 
            }  

            

    }
}