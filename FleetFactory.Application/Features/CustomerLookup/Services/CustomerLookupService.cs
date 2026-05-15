using FleetFactory.Application.Features.CustomerLookup.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Enums;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Features.CustomerLookup.Services
{
    public class CustomerLookupService(
        ICustomerLookupRepository _customerLookupRepository
    ) : ICustomerLookupService
    {
        public async Task<ApiResponse<List<CustomerLookupResponseDto>>> SearchAsync(
            string query,
            CustomerLookupType type)
        {
            if (string.IsNullOrWhiteSpace(query))
                return ApiResponse<List<CustomerLookupResponseDto>>
                    .ErrorResponse("Search query is required");

            var results = await _customerLookupRepository.SearchAsync(query, type);

            return ApiResponse<List<CustomerLookupResponseDto>>
                .SuccessResponse(results, "Customer lookup completed");
        }
    }
}