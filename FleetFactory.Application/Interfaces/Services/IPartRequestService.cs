using FleetFactory.Application.Features.PartRequests.DTOs;
using FleetFactory.Domain.Enums;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IPartRequestService
    {
        Task<ApiResponse<PartRequestResponseDTO>> CreateMyRequestAsync(
            string userId,
            CreatePartRequestRequestDTO request);

        Task<ApiResponse<List<PartRequestResponseDTO>>> GetMyRequestsAsync(string userId);

        Task<ApiResponse<PartRequestResponseDTO>> GetMyRequestByIdAsync(
            string userId,
            Guid requestId);

        Task<ApiResponse<PagedResult<PartRequestResponseDTO>>> GetAllAsync(
            int pageNumber,
            int pageSize);

        Task<ApiResponse<PagedResult<PartRequestResponseDTO>>> SearchAsync(
            string? query,
            PartRequestStatus? status,
            int pageNumber,
            int pageSize);

        Task<ApiResponse<PartRequestResponseDTO>> GetByIdAsync(Guid id);

        Task<ApiResponse<PartRequestResponseDTO>> MarkAsSourcedAsync(
            Guid id,
            UpdatePartRequestStatusDTO request);

        Task<ApiResponse<PartRequestResponseDTO>> RejectAsync(
            Guid id,
            UpdatePartRequestStatusDTO request);
    }
}