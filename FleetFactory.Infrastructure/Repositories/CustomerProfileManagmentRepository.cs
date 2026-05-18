using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FleetFactory.Infrastructure.Repositories
{
    public class CustomerProfileRepository(AppDbContext _context) 
        : ICustomerProfileRepository
    {
        public async Task<CustomerProfile?> GetByUserIdWithVehiclesAsync(string userId)
        {
            return await _context.CustomerProfiles
                .Include(c => c.User)
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(Guid vehicleId)
        {
            return await _context.Vehicles
                .Include(v => v.Customer)
                .FirstOrDefaultAsync(v => v.Id == vehicleId);
        }

        public async Task<bool> VehicleNumberExistsAsync(string vehicleNumber)
        {
            return await _context.Vehicles
                .AnyAsync(v => v.VehicleNumber.ToLower() == vehicleNumber.ToLower());
        }

        public async Task<bool> VehicleNumberExistsExceptIdAsync(
            string vehicleNumber,
            Guid vehicleId)
        {
            return await _context.Vehicles
                .AnyAsync(v =>
                    v.VehicleNumber.ToLower() == vehicleNumber.ToLower() &&
                    v.Id != vehicleId);
        }

        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
        }

        public void UpdateCustomer(CustomerProfile customer)
        {
            _context.CustomerProfiles.Update(customer);
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
        }

        public void DeleteVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}