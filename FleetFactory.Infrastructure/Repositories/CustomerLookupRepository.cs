using FleetFactory.Application.Features.CustomerLookup.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Enums;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class CustomerLookupRepository(AppDbContext _context) : ICustomerLookupRepository
    {
        public async Task<List<CustomerLookupResponseDto>> SearchAsync(
            string query,
            CustomerLookupType type)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<CustomerLookupResponseDto>();

            query = query.Trim().ToLower();

            var customers = _context.CustomerProfiles
                .Include(c => c.User)
                .Include(c => c.Vehicles)
                .AsQueryable();

            customers = type switch
            {
                CustomerLookupType.Name => customers.Where(c =>
                    c.FullName.ToLower().Contains(query)),

                CustomerLookupType.Phone => customers.Where(c =>
                    c.Phone != null &&
                    c.Phone.ToLower().Contains(query)),

                CustomerLookupType.Email => customers.Where(c =>
                    c.User.Email != null &&
                    c.User.Email.ToLower().Contains(query)),

                CustomerLookupType.VehicleNumber => customers.Where(c =>
                    c.Vehicles.Any(v =>
                        v.VehicleNumber.ToLower().Contains(query))),

                CustomerLookupType.CustomerId => customers.Where(c =>
                    c.Id.ToString().ToLower().Contains(query) ||
                    c.UserId.ToLower().Contains(query)),

                _ => customers.Where(c =>
                    c.FullName.ToLower().Contains(query) ||
                    (c.Phone != null &&
                     c.Phone.ToLower().Contains(query)) ||
                    c.Id.ToString().ToLower().Contains(query) ||
                    c.UserId.ToLower().Contains(query) ||
                    (c.User.Email != null &&
                     c.User.Email.ToLower().Contains(query)) ||
                    c.Vehicles.Any(v =>
                        v.VehicleNumber.ToLower().Contains(query)))
            };

            return await customers
                .OrderBy(c => c.FullName)
                .Select(c => new CustomerLookupResponseDto
                {
                    CustomerId = c.Id,
                    UserId = c.UserId,
                    FullName = c.FullName,
                    Email = c.User.Email,
                    Phone = c.Phone,
                    Address = c.Address,
                    Vehicles = c.Vehicles.Select(v =>
                        new CustomerLookupVehicleResponseDto
                        {
                            Id = v.Id,
                            VehicleNumber = v.VehicleNumber,
                            Make = v.Make,
                            Model = v.Model,
                            Year = v.Year
                        }).ToList()
                })
                .ToListAsync();
        }
    }
}