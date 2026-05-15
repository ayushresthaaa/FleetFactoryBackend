using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class PartCategoryRepository(AppDbContext _context) : IPartCategoryRepository
    {
        public async Task<List<PartCategory>> GetAllAsync()
        {
            return await _context.PartCategories
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<PartCategory?> GetByIdAsync(Guid id)
        {
            return await _context.PartCategories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.PartCategories
                .AnyAsync(c => c.Name.ToLower() == name.ToLower().Trim());
        }

        public async Task<bool> ExistsByNameExceptIdAsync(string name, Guid id)
        {
            return await _context.PartCategories
                .AnyAsync(c => c.Name.ToLower() == name.ToLower().Trim() && c.Id != id);
        }

        public async Task AddAsync(PartCategory category)
        {
            await _context.PartCategories.AddAsync(category);
        }

        public void Update(PartCategory category)
        {
            _context.PartCategories.Update(category);
        }

        public void Delete(PartCategory category)
        {
            _context.PartCategories.Remove(category);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}