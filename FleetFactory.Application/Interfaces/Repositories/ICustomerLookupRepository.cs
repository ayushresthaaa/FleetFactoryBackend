using FleetFactory.Application.Features.CustomerLookup.DTOs;
using FleetFactory.Domain.Enums;

namespace FleetFactory.Application.Interfaces.Repositories
{
    public interface ICustomerLookupRepository
    {
        Task<List<CustomerLookupResponseDto>> SearchAsync(
            string query,
            CustomerLookupType type
        );
    }
}