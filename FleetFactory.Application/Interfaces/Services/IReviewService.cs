using FleetFactory.Application.Features.Reviews.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IReviewService
    {
        Task<ApiResponse<ReviewResponseDTO>> CreateMyReviewAsync(
            string userId,
            CreateReviewRequestDTO request);

        Task<ApiResponse<List<ReviewResponseDTO>>> GetMyReviewsAsync(
            string userId);

        Task<ApiResponse<ReviewResponseDTO>> GetMyReviewByIdAsync(
            string userId,
            Guid reviewId);

        Task<ApiResponse<PagedResult<ReviewResponseDTO>>> GetAllAsync(
            int pageNumber,
            int pageSize);

        Task<ApiResponse<List<ReviewResponseDTO>>> GetByCustomerIdAsync(
            Guid customerId);

        Task<ApiResponse<ReviewResponseDTO>> GetByIdAsync(Guid id);

        Task<ApiResponse<ReviewResponseDTO>> HideAsync(Guid id);

        Task<ApiResponse<ReviewResponseDTO>> ShowAsync(Guid id);
    }
}