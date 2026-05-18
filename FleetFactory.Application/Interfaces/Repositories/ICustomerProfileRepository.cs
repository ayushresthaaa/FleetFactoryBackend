using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface ICustomerProfileRepository
    {
        Task<CustomerProfile?> GetByUserIdWithVehiclesAsync(string userId);

        Task<Vehicle?> GetVehicleByIdAsync(Guid vehicleId);

        Task<bool> VehicleNumberExistsAsync(string vehicleNumber);

        Task<bool> VehicleNumberExistsExceptIdAsync(
            string vehicleNumber,
            Guid vehicleId
        );

        Task AddVehicleAsync(Vehicle vehicle);

        void UpdateCustomer(CustomerProfile customer);

        void UpdateVehicle(Vehicle vehicle);

        void DeleteVehicle(Vehicle vehicle);

        Task SaveChangesAsync();
    }
}