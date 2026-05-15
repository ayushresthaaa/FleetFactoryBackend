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

         public async Task<(List<Part> Items, int TotalCount)> SearchAsync(
            string keyword,
            int pageNumber,
            int pageSize)
        {
            keyword = keyword.ToLower();

            var query = _context.Parts
                .Include(p => p.Category)
                .Include(p => p.Vendor)
                .Where(p =>
                    p.IsActive &&
                    (
                        p.Name.ToLower().Contains(keyword) ||
                        p.Sku.ToLower().Contains(keyword) ||
                        (p.Description != null && p.Description.ToLower().Contains(keyword))
                    ))
                .OrderBy(p => p.Name);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<List<Part>> GetLowStockAsync(int threshold)
        {
            return await _context.Parts
                .Include(p => p.Category)
                .Include(p => p.Vendor)
                .Where(p => p.IsActive && p.StockQty < threshold)
                .OrderBy(p => p.StockQty)
                .ToListAsync();
        }

        public async Task<List<Part>> GetAvailableAsync()
        {
            return await _context.Parts
                .Include(p => p.Category)
                .Include(p => p.Vendor)
                .Where(p => p.IsActive && p.StockQty > 0)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<List<StockMovement>> GetStockMovementsAsync(Guid partId)
        {
            return await _context.StockMovements
                .Include(s => s.Part)
                .Where(s => s.PartId == partId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<(List<Part> Items, int TotalCount)> GetByVendorAsync(
            Guid vendorId,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Parts
                .Include(p => p.Category)
                .Include(p => p.Vendor)
                .Where(p => p.IsActive && p.VendorId == vendorId)
                .OrderBy(p => p.Name);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<(List<Part> Items, int TotalCount)> GetByCategoryAsync(
            Guid categoryId,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Parts
                .Include(p => p.Category)
                .Include(p => p.Vendor)
                .Where(p =>
                    p.IsActive &&
                    p.CategoryId == categoryId)
                .OrderBy(p => p.Name);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

    }
}