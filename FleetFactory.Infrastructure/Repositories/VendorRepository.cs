using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using FleetFactory.Infrastructure.Helpers; 
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class VendorRepository(AppDbContext _context) : IVendorRepository
    {
        public async Task<(List<Vendor> Vendors, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            bool includeInactive = false)
        {
            var query = _context.Vendors.AsQueryable();

            if (!includeInactive)
                query = query.Where(v => v.IsActive);

            var totalCount = await query.CountAsync();

            var vendors = await query
                .OrderBy(v => v.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (vendors, totalCount);
        }

        public async Task<Vendor?> GetByIdAsync(Guid id)
        {
            return await _context.Vendors
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Vendors
                .AnyAsync(v => v.Name.ToLower() == name.ToLower());
        }

        public async Task AddAsync(Vendor vendor)
        {
            await _context.Vendors.AddAsync(vendor);
        }

        public void Update(Vendor vendor)
        {
            _context.Vendors.Update(vendor);
        }

        public async Task SoftDeleteAsync(Guid id)
        {
            var vendor = await _context.Vendors.FindAsync(id);

            if (vendor == null)
                return;

            vendor.IsActive = false;
            vendor.UpdatedAt = DateTimeHelper.UtcNow;

            _context.Vendors.Update(vendor);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}