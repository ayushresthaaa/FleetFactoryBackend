using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class StaffRepository(AppDbContext _context) : IStaffRepository
    {
        public async Task<(List<StaffProfile> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            var query = _context.StaffProfiles
                .OrderBy(s => s.FullName);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<StaffProfile?> GetByIdAsync(Guid id)
        {
            return await _context.StaffProfiles
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<StaffProfile?> GetByUserIdAsync(string userId)
        {
            return await _context.StaffProfiles
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task AddAsync(StaffProfile staffProfile)
        {
            await _context.StaffProfiles.AddAsync(staffProfile);
        }

        public void Update(StaffProfile staffProfile)
        {
            _context.StaffProfiles.Update(staffProfile);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}