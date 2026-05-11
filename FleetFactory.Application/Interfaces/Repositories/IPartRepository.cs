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

        Task AddStockMovementAsync(StockMovement stockMovement);

        Task<bool> ExistsBySkuAsync(string sku);

        Task<bool> ExistsBySkuExceptIdAsync(string sku, Guid id);

        Task<(List<Part> Items, int TotalCount)> SearchAsync(string keyword, int pageNumber, int pageSize);

        Task<List<Part>> GetLowStockAsync(int threshold);

        Task<List<Part>> GetAvailableAsync();

        Task<List<StockMovement>> GetStockMovementsAsync(Guid partId);

        Task SaveChangesAsync();
    }
}