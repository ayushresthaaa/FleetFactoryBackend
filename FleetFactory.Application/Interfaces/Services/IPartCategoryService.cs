using FleetFactory.Application.Features.PartCategories.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IPartCategoryService
    {
        Task<ApiResponse<List<PartCategoryResponseDto>>> GetAllAsync();
        Task<ApiResponse<PartCategoryResponseDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<PartCategoryResponseDto>> CreateAsync(CreatePartCategoryRequestDto request);
        Task<ApiResponse<PartCategoryResponseDto>> UpdateAsync(Guid id, UpdatePartCategoryRequestDto request);
        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}