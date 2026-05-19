using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class OverdueCreditRepository(AppDbContext _context) : IOverdueCreditRepository
    {
        public async Task<List<CustomerProfile>> GetOverdueCreditCustomersAsync(DateTime thresholdDate)
        {
            return await _context.CustomerProfiles
                .Include(c => c.User)
                .Where(c =>
                    c.CreditBalance > 0 &&
                    c.UpdatedAt <= thresholdDate
                )
                .OrderByDescending(c => c.CreditBalance)
                .ToListAsync();
        }

        public async Task<CustomerProfile?> GetOverdueCreditCustomerByIdAsync(
            Guid customerId,
            DateTime thresholdDate
        )
        {
            return await _context.CustomerProfiles
                .Include(c => c.User)
                .FirstOrDefaultAsync(c =>
                    c.Id == customerId &&
                    c.CreditBalance > 0 &&
                    c.UpdatedAt <= thresholdDate
                );
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}