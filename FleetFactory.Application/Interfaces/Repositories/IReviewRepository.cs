using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<(List<Review> Reviews, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize);

        Task<List<Review>> GetByCustomerIdAsync(Guid customerId);

        Task<Review?> GetByIdAsync(Guid id);

        Task<Review?> GetMyReviewByIdAsync(Guid customerId, Guid reviewId);

        Task AddAsync(Review review);

        void Update(Review review);

        Task SaveChangesAsync();
    }
}