using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface IPartRepository
    {
        Task<List<Part>> GetAllAsync();

        Task<(List<Part> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        
        Task<Part?> GetByIdAsync(Guid id);
        Task<Part?> GetBySkuAsync(string sku);

        Task AddAsync(Part part);
        void Update(Part part);
        void Delete(Part part);

        Task<bool> ExistsBySkuAsync(string sku);
        Task<bool> ExistsBySkuExceptIdAsync(string sku, Guid id);

        Task SaveChangesAsync();
    }
}