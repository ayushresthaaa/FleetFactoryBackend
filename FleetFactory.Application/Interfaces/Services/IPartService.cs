using FleetFactory.Application.Features.Parts.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IPartService
    {
        Task<ApiResponse<PagedResult<PartResponseDto>>> GetAllAsync(int pageNumber, int pageSize);

        Task<ApiResponse<PartResponseDto>> GetByIdAsync(Guid id);

        Task<ApiResponse<PartResponseDto>> CreateAsync(CreatePartRequestDto request);

        Task<ApiResponse<PartResponseDto>> UpdateAsync(Guid id, UpdatePartRequestDto request);

        Task<ApiResponse<string>> DeleteAsync(Guid id);
    }
}