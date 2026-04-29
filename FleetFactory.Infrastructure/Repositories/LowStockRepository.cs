using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class LowStockRepository(AppDbContext _context) : ILowStockRepository
    {
        public async Task<List<Part>> GetLowStockPartsAsync(int threshold)
        {
            return await _context.Parts
                .Where(p => p.IsActive && p.StockQty < threshold)
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