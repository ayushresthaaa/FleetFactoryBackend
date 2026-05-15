using FleetFactory.Application.Features.CustomerLookup.DTOs;
using FleetFactory.Domain.Enums;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface ICustomerLookupService
    {
        Task<ApiResponse<List<CustomerLookupResponseDto>>> SearchAsync(
            string query,
            CustomerLookupType type
        );
    }
}