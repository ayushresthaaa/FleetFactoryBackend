using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class CustomerSideRepository(AppDbContext _context) : ICustomerSideRepository
    {
        public async Task<CustomerProfile?> GetCustomerByUserIdAsync(string userId)
        {
            return await _context.CustomerProfiles
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<List<SalesInvoice>> GetPurchaseHistoryAsync(Guid customerId)
        {
            return await _context.SalesInvoices
                .Include(s => s.Vehicle)
                .Include(s => s.Items)
                    .ThenInclude(i => i.Part)
                .Where(s => s.CustomerId == customerId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentHistoryAsync(Guid customerId)
        {
            return await _context.Appointments
                .Include(a => a.Vehicle)
                .Where(a => a.CustomerId == customerId)
                .OrderByDescending(a => a.ScheduledAt)
                .ToListAsync();
        }
    }
}