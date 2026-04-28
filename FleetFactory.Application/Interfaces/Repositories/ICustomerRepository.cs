using FleetFactory.Domain.Entities;
namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        Task<(List<CustomerProfile> Customers, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<CustomerProfile?> GetByIdAsync(Guid id);
        Task<bool> VehicleNumberExistsAsync(string vehicleNumber);
        Task AddAsync(CustomerProfile customer);
        void Update(CustomerProfile customer);
        Task SaveChangesAsync();
        Task AddVehicleAsync(Vehicle vehicle);
   
    }
}