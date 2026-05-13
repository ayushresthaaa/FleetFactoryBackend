using FleetFactory.Domain.Entities;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface ICustomerMailRepository
    {
        Task<CustomerMail?> GetByEmailAsync(string email);
    }
}