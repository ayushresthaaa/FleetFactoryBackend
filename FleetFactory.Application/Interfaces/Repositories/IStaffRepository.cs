using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface IStaffRepository
    {
        Task<(List<StaffProfile> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);

        Task<StaffProfile?> GetByIdAsync(Guid id);

        Task<StaffProfile?> GetByUserIdAsync(string userId);

        Task AddAsync(StaffProfile staffProfile);

        void Update(StaffProfile staffProfile);

        Task SaveChangesAsync();
    }
}