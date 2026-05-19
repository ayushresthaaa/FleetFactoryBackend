using FleetFactory.Application.Features.Reviews.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Domain.Enums;
using FleetFactory.Infrastructure.Helpers;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Features.Reviews.Services
{
    public class ReviewService(
        IReviewRepository _reviewRepository,
        IAppointmentRepository _appointmentRepository,
        ICustomerProfileRepository _customerProfileRepository
    ) : IReviewService
    {
        public async Task<ApiResponse<ReviewResponseDTO>> CreateMyReviewAsync(
            string userId,
            CreateReviewRequestDTO request)
        {
            var customer = await _customerProfileRepository
                .GetByUserIdWithVehiclesAsync(userId);

            if (customer == null)
                return ApiResponse<ReviewResponseDTO>
                    .ErrorResponse("Customer profile not found");

            var appointment = await _appointmentRepository
                .GetByIdAsync(request.AppointmentId);

            if (appointment == null)
                return ApiResponse<ReviewResponseDTO>
                    .ErrorResponse("Appointment not found");

            if (appointment.CustomerId != customer.Id)
                return ApiResponse<ReviewResponseDTO>
                    .ErrorResponse("This appointment does not belong to you");

            if (appointment.Status != AppointmentStatus.Completed)
                return ApiResponse<ReviewResponseDTO>
                    .ErrorResponse("Only completed appointments can be reviewed");

            if (appointment.Review != null)
                return ApiResponse<ReviewResponseDTO>
                    .ErrorResponse("This appointment already has a review");

            if (request.Rating < 1 || request.Rating > 5)
                return ApiResponse<ReviewResponseDTO>
                    .ErrorResponse("Rating must be between 1 and 5");

            var review = new Review
            {
                CustomerId = customer.Id,
                AppointmentId = appointment.Id,
                Rating = request.Rating,
                Comment = request.Comment,
                IsVisible = true,
                CreatedAt = DateTimeHelper.UtcNow
            };

            await _reviewRepository.AddAsync(review);
            await _reviewRepository.SaveChangesAsync();

            return await GetByIdAsync(review.Id);
        }

        public async Task<ApiResponse<List<ReviewResponseDTO>>> GetMyReviewsAsync(
            string userId)
        {
            var customer = await _customerProfileRepository
                .GetByUserIdWithVehiclesAsync(userId);

            if (customer == null)
                return ApiResponse<List<ReviewResponseDTO>>
                    .ErrorResponse("Customer profile not found");

            var reviews = await _reviewRepository
                .GetByCustomerIdAsync(customer.Id);

            var response = reviews.Select(MapToResponse).ToList();

            return ApiResponse<List<ReviewResponseDTO>>
                .SuccessResponse(response, "Reviews retrieved successfully");
        }

        public async Task<ApiResponse<ReviewResponseDTO>> GetMyReviewByIdAsync(
            string userId,
            Guid reviewId)
        {
            var customer = await _customerProfileRepository
                .GetByUserIdWithVehiclesAsync(userId);

            if (customer == null)
                return ApiResponse<ReviewResponseDTO>
                    .ErrorResponse("Customer profile not found");

            var review = await _reviewRepository
                .GetMyReviewByIdAsync(customer.Id, reviewId);

            if (review == null)
                return ApiResponse<ReviewResponseDTO>
                    .ErrorResponse("Review not found");

            return ApiResponse<ReviewResponseDTO>
                .SuccessResponse(MapToResponse(review), "Review fetched successfully");
        }

        public async Task<ApiResponse<PagedResult<ReviewResponseDTO>>> GetAllAsync(
            int pageNumber,
            int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var (reviews, totalCount) = await _reviewRepository
                .GetPagedAsync(pageNumber, pageSize);

            var response = reviews.Select(MapToResponse).ToList();

            var paged = PagedResult<ReviewResponseDTO>.Create(
                response,
                pageNumber,
                pageSize,
                totalCount
            );

            return ApiResponse<PagedResult<ReviewResponseDTO>>
                .SuccessResponse(paged, "Reviews retrieved successfully");
        }

        public async Task<ApiResponse<List<ReviewResponseDTO>>> GetByCustomerIdAsync(
            Guid customerId)
        {
            var reviews = await _reviewRepository
                .GetByCustomerIdAsync(customerId);

            var response = reviews.Select(MapToResponse).ToList();

            return ApiResponse<List<ReviewResponseDTO>>
                .SuccessResponse(response, "Customer reviews retrieved successfully");
        }

        public async Task<ApiResponse<ReviewResponseDTO>> GetByIdAsync(Guid id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);

            if (review == null)
                return ApiResponse<ReviewResponseDTO>
                    .ErrorResponse("Review not found");

            return ApiResponse<ReviewResponseDTO>
                .SuccessResponse(MapToResponse(review), "Review fetched successfully");
        }

        public async Task<ApiResponse<ReviewResponseDTO>> HideAsync(Guid id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);

            if (review == null)
                return ApiResponse<ReviewResponseDTO>
                    .ErrorResponse("Review not found");

            review.IsVisible = false;

            _reviewRepository.Update(review);
            await _reviewRepository.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<ApiResponse<ReviewResponseDTO>> ShowAsync(Guid id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);

            if (review == null)
                return ApiResponse<ReviewResponseDTO>
                    .ErrorResponse("Review not found");

            review.IsVisible = true;

            _reviewRepository.Update(review);
            await _reviewRepository.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        private static ReviewResponseDTO MapToResponse(Review review)
        {
            return new ReviewResponseDTO
            {
                Id = review.Id,
                CustomerId = review.CustomerId,
                CustomerName = review.Customer?.FullName ?? "",
                AppointmentId = review.AppointmentId,
                AppointmentDate = review.Appointment?.ScheduledAt ?? DateTime.UtcNow,
                Rating = review.Rating,
                Comment = review.Comment,
                IsVisible = review.IsVisible,
                CreatedAt = review.CreatedAt
            };
        }
    }
}