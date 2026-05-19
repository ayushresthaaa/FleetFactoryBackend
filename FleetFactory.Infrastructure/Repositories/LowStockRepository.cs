using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class LowStockRepository(AppDbContext _context) : ILowStockRepository
    {
        public async Task<List<Part>> GetLowStockPartsAsync()
        {
            return await _context.Parts
                .Include(p => p.Category)
                .Where(p =>
                    p.IsActive &&
                    p.Category != null &&
                    p.StockQty < p.Category.LowStockThreshold)
                .OrderBy(p => p.StockQty)
                .ToListAsync();
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