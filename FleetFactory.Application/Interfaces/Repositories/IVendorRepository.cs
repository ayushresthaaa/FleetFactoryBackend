using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface IVendorRepository
    {
        Task<(List<Vendor> Vendors, int TotalCount)> GetPagedAsync(
            int pageNumber, 
            int pageSize, 
            bool includeInactive = false
        );

        Task<Vendor?> GetByIdAsync(Guid id);

        Task<bool> ExistsByNameAsync(string name);

        Task AddAsync(Vendor vendor);

        void Update(Vendor vendor);

        Task SoftDeleteAsync(Guid id);

        Task SaveChangesAsync();
    }
}