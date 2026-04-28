using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class CustomerRepository(AppDbContext _context) : ICustomerRepository
    {
        public async Task<(List<CustomerProfile> Customers, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            var query = _context.CustomerProfiles
                .Include(c => c.Vehicles)
                .OrderBy(c => c.FullName);

            var totalCount = await query.CountAsync();

            var customers = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (customers, totalCount);
        }

        public async Task<CustomerProfile?> GetByIdAsync(Guid id)
        {
            return await _context.CustomerProfiles
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> VehicleNumberExistsAsync(string vehicleNumber)
        {
            return await _context.Vehicles
                .AnyAsync(v => v.VehicleNumber.ToLower() == vehicleNumber.ToLower().Trim());
        }

        public async Task AddAsync(CustomerProfile customer)
        {
            await _context.CustomerProfiles.AddAsync(customer);
        }

        public void Update(CustomerProfile customer)
        {
            _context.CustomerProfiles.Update(customer);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
        }

        //rachina part
        public async Task<CustomerProfile?> GetWithHistoryAsync(Guid id)
        {
            return await _context.CustomerProfiles
                .Include(c => c.Vehicles)
                .Include(c => c.SalesInvoices)
                    .ThenInclude(s => s.Items)
                        .ThenInclude(i => i.Part)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}