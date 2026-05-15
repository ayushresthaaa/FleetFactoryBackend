using FleetFactory.Application.Features.Customers.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        Task<ApiResponse<CustomerResponseDto>> CreateAsync(CreateCustomerWithVehicleRequestDto request);

        Task<ApiResponse<PagedResult<CustomerResponseDto>>> GetAllAsync(int pageNumber, int pageSize);

        Task<ApiResponse<CustomerResponseDto>> GetByIdAsync(Guid id);

        Task<ApiResponse<CustomerResponseDto>> AddVehicleAsync(Guid customerId, AddVehicleRequestDto request);

        //added by rachina

        Task<ApiResponse<CustomerHistoryResponseDTO>> 
            GetCustomerHistoryAsync(Guid customerId);

        //rabison part
        Task<ApiResponse<List<CustomerSearchResponseDto>>> SearchAsync(string query);
    }
}