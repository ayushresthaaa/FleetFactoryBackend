using FleetFactory.Application.Features.Vendors.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IVendorService
    {
        Task<ApiResponse<PagedResult<VendorResponseDto>>> GetAllAsync(int pageNumber, int pageSize);
        Task<ApiResponse<VendorResponseDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<VendorResponseDto>> CreateAsync(CreateVendorRequestDto request);
        Task<ApiResponse<VendorResponseDto>> UpdateAsync(Guid id, UpdateVendorRequestDto request);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}