using FleetFactory.Domain.Entities;
using FleetFactory.Domain.Enums;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface IPartRequestRepository
    {
        Task<(List<PartRequest> Requests, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize);

        Task<(List<PartRequest> Requests, int TotalCount)> SearchAsync(
            string? query,
            PartRequestStatus? status,
            int pageNumber,
            int pageSize);

        Task<List<PartRequest>> GetMyRequestsAsync(Guid customerId);

        Task<PartRequest?> GetByIdAsync(Guid id);

        Task<PartRequest?> GetMyRequestByIdAsync(Guid customerId, Guid requestId);

        Task AddAsync(PartRequest request);

        void Update(PartRequest request);

        Task SaveChangesAsync();
    }
}