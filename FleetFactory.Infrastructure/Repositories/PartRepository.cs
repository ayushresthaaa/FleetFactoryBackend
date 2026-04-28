using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class PartRepository(AppDbContext _context) : IPartRepository
    {
        public async Task<List<Part>> GetAllAsync()
        {
            return await _context.Parts
                .Include(p => p.Category)
                .Include(p => p.Vendor)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<(List<Part> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Parts
                .Include(p => p.Category)
                .Include(p => p.Vendor)
                .OrderBy(p => p.Name);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Part?> GetByIdAsync(Guid id)
        {
            return await _context.Parts
                .Include(p => p.Category)
                .Include(p => p.Vendor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Part?> GetBySkuAsync(string sku)
        {
            return await _context.Parts
                .Include(p => p.Category)
                .Include(p => p.Vendor)
                .FirstOrDefaultAsync(p => p.Sku == sku);
        }

        public async Task AddAsync(Part part)
        {
            await _context.Parts.AddAsync(part);
        }

        public void Update(Part part)
        {
            _context.Parts.Update(part);
        }

         public void Delete(Part part)
        {
            _context.Parts.Remove(part);
        }

        public async Task<bool> ExistsBySkuAsync(string sku)
        {
            return await _context.Parts.AnyAsync(p => p.Sku == sku);
        }

        //this means any part with same SKU but different ID exists, used for update validation
        public async Task<bool> ExistsBySkuExceptIdAsync(string sku, Guid id)
        {
            return await _context.Parts
                .AnyAsync(p => p.Sku == sku && p.Id != id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

          public async Task AddStockMovementAsync(StockMovement stockMovement)
        {
            await _context.StockMovements.AddAsync(stockMovement);
        }
    }
}