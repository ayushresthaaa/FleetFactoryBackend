using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Domain.Enums;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class PartRequestRepository(AppDbContext _context)
        : IPartRequestRepository
    {
        public async Task<(List<PartRequest> Requests, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            var query = _context.PartRequests
                .Include(p => p.Customer)
                    .ThenInclude(c => c.User)
                .Include(p => p.Vehicle)
                .Include(p => p.Part)
                .OrderByDescending(p => p.CreatedAt);

            var totalCount = await query.CountAsync();

            var requests = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (requests, totalCount);
        }

        public async Task<(List<PartRequest> Requests, int TotalCount)> SearchAsync(
            string? query,
            PartRequestStatus? status,
            int pageNumber,
            int pageSize)
        {
            var requestsQuery = _context.PartRequests
                .Include(p => p.Customer)
                    .ThenInclude(c => c.User)
                .Include(p => p.Vehicle)
                .Include(p => p.Part)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var search = query.Trim().ToLower();

                requestsQuery = requestsQuery.Where(p =>
                    p.PartName.ToLower().Contains(search) ||
                    (p.Description != null && p.Description.ToLower().Contains(search)) ||
                    p.Customer.FullName.ToLower().Contains(search) ||
                    (p.Vehicle != null &&
                     p.Vehicle.VehicleNumber.ToLower().Contains(search)) ||
                    (p.Part != null &&
                     p.Part.Name.ToLower().Contains(search))
                );
            }

            if (status.HasValue)
            {
                requestsQuery = requestsQuery
                    .Where(p => p.Status == status.Value);
            }

            requestsQuery = requestsQuery
                .OrderByDescending(p => p.CreatedAt);

            var totalCount = await requestsQuery.CountAsync();

            var requests = await requestsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (requests, totalCount);
        }

        public async Task<List<PartRequest>> GetMyRequestsAsync(Guid customerId)
        {
            return await _context.PartRequests
                .Include(p => p.Customer)
                    .ThenInclude(c => c.User)
                .Include(p => p.Vehicle)
                .Include(p => p.Part)
                .Where(p => p.CustomerId == customerId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<PartRequest?> GetByIdAsync(Guid id)
        {
            return await _context.PartRequests
                .Include(p => p.Customer)
                    .ThenInclude(c => c.User)
                .Include(p => p.Vehicle)
                .Include(p => p.Part)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PartRequest?> GetMyRequestByIdAsync(
            Guid customerId,
            Guid requestId)
        {
            return await _context.PartRequests
                .Include(p => p.Customer)
                    .ThenInclude(c => c.User)
                .Include(p => p.Vehicle)
                .Include(p => p.Part)
                .FirstOrDefaultAsync(p =>
                    p.Id == requestId &&
                    p.CustomerId == customerId);
        }

        public async Task AddAsync(PartRequest request)
        {
            await _context.PartRequests.AddAsync(request);
        }

        public void Update(PartRequest request)
        {
            _context.PartRequests.Update(request);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}