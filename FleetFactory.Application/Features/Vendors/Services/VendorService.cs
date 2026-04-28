using FleetFactory.Application.Features.Vendors.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Helpers;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Features.Vendors.Services
{
    public class VendorService(IVendorRepository _vendorRepository) : IVendorService
    {
        public async Task<ApiResponse<PagedResult<VendorResponseDto>>> GetAllAsync(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var (vendors, totalCount) = await _vendorRepository.GetPagedAsync(pageNumber, pageSize);

            var response = vendors.Select(v => new VendorResponseDto
            {
                Id = v.Id,
                Name = v.Name,
                ContactName = v.ContactName,
                Phone = v.Phone,
                Email = v.Email,
                Address = v.Address,
                IsActive = v.IsActive,
                CreatedAt = v.CreatedAt
            }).ToList();

            var pagedResult = PagedResult<VendorResponseDto>.Create(
                response,
                pageNumber,
                pageSize,
                totalCount
            );

            return ApiResponse<PagedResult<VendorResponseDto>>
                .SuccessResponse(pagedResult, "Vendors retrieved successfully");
        }

        public async Task<ApiResponse<VendorResponseDto>> GetByIdAsync(Guid id)
        {
            var vendor = await _vendorRepository.GetByIdAsync(id);

            if (vendor == null)
                return ApiResponse<VendorResponseDto>.ErrorResponse("Vendor not found");

            var response = new VendorResponseDto
            {
                Id = vendor.Id,
                Name = vendor.Name,
                ContactName = vendor.ContactName,
                Phone = vendor.Phone,
                Email = vendor.Email,
                Address = vendor.Address,
                IsActive = vendor.IsActive,
                CreatedAt = vendor.CreatedAt
            };

            return ApiResponse<VendorResponseDto>
                .SuccessResponse(response, "Vendor fetched successfully");
        }

        public async Task<ApiResponse<VendorResponseDto>> CreateAsync(CreateVendorRequestDto request)
        {
            var exists = await _vendorRepository.ExistsByNameAsync(request.Name);

            if (exists)
                return ApiResponse<VendorResponseDto>.ErrorResponse("Vendor name already exists");

            var vendor = new Vendor
            {
                Name = request.Name.Trim(),
                ContactName = request.ContactName,
                Phone = request.Phone,
                Email = request.Email,
                Address = request.Address,
                IsActive = true,
                CreatedAt = DateTimeHelper.UtcNow,
                UpdatedAt = DateTimeHelper.UtcNow
            };

            await _vendorRepository.AddAsync(vendor);
            await _vendorRepository.SaveChangesAsync();

            var response = new VendorResponseDto
            {
                Id = vendor.Id,
                Name = vendor.Name,
                ContactName = vendor.ContactName,
                Phone = vendor.Phone,
                Email = vendor.Email,
                Address = vendor.Address,
                IsActive = vendor.IsActive,
                CreatedAt = vendor.CreatedAt
            };

            return ApiResponse<VendorResponseDto>
                .SuccessResponse(response, "Vendor created successfully");
        }

        public async Task<ApiResponse<VendorResponseDto>> UpdateAsync(Guid id, UpdateVendorRequestDto request)
        {
            var vendor = await _vendorRepository.GetByIdAsync(id);

            if (vendor == null)
                return ApiResponse<VendorResponseDto>.ErrorResponse("Vendor not found");

            vendor.Name = request.Name.Trim();
            vendor.ContactName = request.ContactName;
            vendor.Phone = request.Phone;
            vendor.Email = request.Email;
            vendor.Address = request.Address;
            vendor.IsActive = request.IsActive;
            vendor.UpdatedAt = DateTimeHelper.UtcNow;

            _vendorRepository.Update(vendor);
            await _vendorRepository.SaveChangesAsync();

            var response = new VendorResponseDto
            {
                Id = vendor.Id,
                Name = vendor.Name,
                ContactName = vendor.ContactName,
                Phone = vendor.Phone,
                Email = vendor.Email,
                Address = vendor.Address,
                IsActive = vendor.IsActive,
                CreatedAt = vendor.CreatedAt
            };

            return ApiResponse<VendorResponseDto>
                .SuccessResponse(response, "Vendor updated successfully");
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            var vendor = await _vendorRepository.GetByIdAsync(id);

            if (vendor == null)
                return ApiResponse<string>.ErrorResponse("Vendor not found");

            await _vendorRepository.SoftDeleteAsync(id);
            await _vendorRepository.SaveChangesAsync();

            return ApiResponse<string>
                .SuccessResponse("Deleted", "Vendor deleted successfully");
        }
    }
}