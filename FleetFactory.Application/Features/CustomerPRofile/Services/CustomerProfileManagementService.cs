using FleetFactory.Application.Features.CustomerProfileManagement.DTOs;
using FleetFactory.Application.Features.Customers.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Helpers;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Features.CustomerProfileManagement.Services
{
    public class CustomerProfileService(
        ICustomerProfileRepository _customerProfileRepository
    ) : ICustomerProfileService
    {
        public async Task<ApiResponse<CustomerResponseDto>> GetMyProfileAsync(string userId)
        {
            var customer = await _customerProfileRepository.GetByUserIdWithVehiclesAsync(userId);

            if (customer == null)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Customer profile not found");

            return ApiResponse<CustomerResponseDto>
                .SuccessResponse(MapToResponse(customer), "Profile retrieved successfully");
        }

        public async Task<ApiResponse<CustomerResponseDto>> UpdateMyProfileAsync(
            string userId,
            UpdateMyProfileRequestDto request)
        {
            var customer = await _customerProfileRepository.GetByUserIdWithVehiclesAsync(userId);

            if (customer == null)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Customer profile not found");

            if (string.IsNullOrWhiteSpace(request.FullName))
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Full name is required");

            customer.FullName = request.FullName.Trim();
            customer.Phone = request.Phone;
            customer.Address = request.Address;
            customer.UpdatedAt = DateTimeHelper.UtcNow;

            _customerProfileRepository.UpdateCustomer(customer);
            await _customerProfileRepository.SaveChangesAsync();

            return ApiResponse<CustomerResponseDto>
                .SuccessResponse(MapToResponse(customer), "Profile updated successfully");
        }

        public async Task<ApiResponse<CustomerResponseDto>> AddMyVehicleAsync(
            string userId,
            AddMyVehicleRequestDto request)
        {
            var customer = await _customerProfileRepository.GetByUserIdWithVehiclesAsync(userId);

            if (customer == null)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Customer profile not found");

            if (string.IsNullOrWhiteSpace(request.VehicleNumber))
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Vehicle number is required");

            var exists = await _customerProfileRepository.VehicleNumberExistsAsync(
                request.VehicleNumber.Trim()
            );

            if (exists)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Vehicle number already exists");

            var vehicle = new Vehicle
            {
                CustomerId = customer.Id,
                VehicleNumber = request.VehicleNumber.Trim(),
                Make = request.Make,
                Model = request.Model,
                Year = request.Year,
                CreatedAt = DateTimeHelper.UtcNow,
                UpdatedAt = DateTimeHelper.UtcNow
            };

            await _customerProfileRepository.AddVehicleAsync(vehicle);

            customer.UpdatedAt = DateTimeHelper.UtcNow;
            _customerProfileRepository.UpdateCustomer(customer);

            await _customerProfileRepository.SaveChangesAsync();

            return await GetMyProfileAsync(userId);
        }

        public async Task<ApiResponse<CustomerResponseDto>> UpdateMyVehicleAsync(
            string userId,
            Guid vehicleId,
            UpdateMyVehicleRequestDto request)
        {
            var customer = await _customerProfileRepository.GetByUserIdWithVehiclesAsync(userId);

            if (customer == null)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Customer profile not found");

            var vehicle = await _customerProfileRepository.GetVehicleByIdAsync(vehicleId);

            if (vehicle == null)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Vehicle not found");

            if (vehicle.CustomerId != customer.Id)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Vehicle does not belong to this customer");

            if (string.IsNullOrWhiteSpace(request.VehicleNumber))
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Vehicle number is required");

            var exists = await _customerProfileRepository.VehicleNumberExistsExceptIdAsync(
                request.VehicleNumber.Trim(),
                vehicleId
            );

            if (exists)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Vehicle number already exists");

            vehicle.VehicleNumber = request.VehicleNumber.Trim();
            vehicle.Make = request.Make;
            vehicle.Model = request.Model;
            vehicle.Year = request.Year;
            vehicle.UpdatedAt = DateTimeHelper.UtcNow;

            customer.UpdatedAt = DateTimeHelper.UtcNow;

            _customerProfileRepository.UpdateVehicle(vehicle);
            _customerProfileRepository.UpdateCustomer(customer);

            await _customerProfileRepository.SaveChangesAsync();

            return await GetMyProfileAsync(userId);
        }

        public async Task<ApiResponse<CustomerResponseDto>> DeleteMyVehicleAsync(
            string userId,
            Guid vehicleId)
        {
            var customer = await _customerProfileRepository.GetByUserIdWithVehiclesAsync(userId);

            if (customer == null)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Customer profile not found");

            var vehicle = await _customerProfileRepository.GetVehicleByIdAsync(vehicleId);

            if (vehicle == null)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Vehicle not found");

            if (vehicle.CustomerId != customer.Id)
                return ApiResponse<CustomerResponseDto>.ErrorResponse("Vehicle does not belong to this customer");

            _customerProfileRepository.DeleteVehicle(vehicle);

            customer.UpdatedAt = DateTimeHelper.UtcNow;
            _customerProfileRepository.UpdateCustomer(customer);

            await _customerProfileRepository.SaveChangesAsync();

            return await GetMyProfileAsync(userId);
        }

        private static CustomerResponseDto MapToResponse(CustomerProfile customer)
        {
            return new CustomerResponseDto
            {
                Id = customer.Id,
                UserId = customer.UserId,
                Email = customer.User?.Email ?? "",
                FullName = customer.FullName,
                Phone = customer.Phone,
                Address = customer.Address,
                CreditBalance = customer.CreditBalance,
                CreatedAt = customer.CreatedAt,
                Vehicles = customer.Vehicles.Select(v => new VehicleResponseDto
                {
                    Id = v.Id,
                    VehicleNumber = v.VehicleNumber,
                    Make = v.Make,
                    Model = v.Model,
                    Year = v.Year
                }).ToList()
            };
        }
    }
}