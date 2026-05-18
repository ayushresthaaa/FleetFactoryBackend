using FleetFactory.Application.Features.CustomerProfileManagement.DTOs;
using FleetFactory.Application.Features.Customers.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface ICustomerProfileService
    {
        Task<ApiResponse<CustomerResponseDto>> GetMyProfileAsync(string userId);

        Task<ApiResponse<CustomerResponseDto>> UpdateMyProfileAsync(
            string userId,
            UpdateMyProfileRequestDto request
        );

        Task<ApiResponse<CustomerResponseDto>> AddMyVehicleAsync(
            string userId,
            AddMyVehicleRequestDto request
        );

        Task<ApiResponse<CustomerResponseDto>> UpdateMyVehicleAsync(
            string userId,
            Guid vehicleId,
            UpdateMyVehicleRequestDto request
        );

        Task<ApiResponse<CustomerResponseDto>> DeleteMyVehicleAsync(
            string userId,
            Guid vehicleId
        );
    }
}