using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface IPartCategoryRepository
    {
        Task<List<PartCategory>> GetAllAsync();
        Task<PartCategory?> GetByIdAsync(Guid id);
        Task<bool> ExistsByNameAsync(string name);
        Task<bool> ExistsByNameExceptIdAsync(string name, Guid id);
        Task AddAsync(PartCategory category);
        void Update(PartCategory category);
        void Delete(PartCategory category);
        Task SaveChangesAsync();
    }
}