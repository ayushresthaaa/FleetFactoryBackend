using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class ReviewRepository(AppDbContext _context) : IReviewRepository
    {
        public async Task<(List<Review> Reviews, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            var query = _context.Reviews
                .Include(r => r.Customer)
                .Include(r => r.Appointment)
                .OrderByDescending(r => r.CreatedAt);

            var totalCount = await query.CountAsync();

            var reviews = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (reviews, totalCount);
        }

        public async Task<List<Review>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.Reviews
                .Include(r => r.Customer)
                .Include(r => r.Appointment)
                .Where(r => r.CustomerId == customerId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(Guid id)
        {
            return await _context.Reviews
                .Include(r => r.Customer)
                .Include(r => r.Appointment)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Review?> GetMyReviewByIdAsync(Guid customerId, Guid reviewId)
        {
            return await _context.Reviews
                .Include(r => r.Customer)
                .Include(r => r.Appointment)
                .FirstOrDefaultAsync(r => r.Id == reviewId && r.CustomerId == customerId);
        }

        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public void Update(Review review)
        {
            _context.Reviews.Update(review);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}