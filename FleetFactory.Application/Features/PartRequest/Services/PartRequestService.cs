using FleetFactory.Application.Features.PartRequests.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Domain.Enums;
using FleetFactory.Infrastructure.Helpers;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Features.PartRequests.Services
{
    public class PartRequestService(
        IPartRequestRepository _partRequestRepository,
        ICustomerProfileRepository _customerProfileRepository,
        IEmailService _emailService,
        IPartRepository _partRepository
    ) : IPartRequestService
    {
        private const int RequestableStockThreshold = 10;

        public async Task<ApiResponse<PartRequestResponseDTO>> CreateMyRequestAsync(
            string userId,
            CreatePartRequestRequestDTO request)
        {
            var customer = await _customerProfileRepository.GetByUserIdWithVehiclesAsync(userId);

            if (customer == null)
                return ApiResponse<PartRequestResponseDTO>.ErrorResponse("Customer profile not found");

            if (!request.PartId.HasValue && string.IsNullOrWhiteSpace(request.PartName))
                return ApiResponse<PartRequestResponseDTO>.ErrorResponse("Part name is required");

            if (request.VehicleId.HasValue &&
                !customer.Vehicles.Any(v => v.Id == request.VehicleId.Value))
            {
                return ApiResponse<PartRequestResponseDTO>
                    .ErrorResponse("Vehicle does not belong to this customer");
            }

            Part? existingPart = null;

            if (request.PartId.HasValue)
            {
                existingPart = await _partRepository.GetByIdAsync(request.PartId.Value);

                if (existingPart == null)
                    return ApiResponse<PartRequestResponseDTO>.ErrorResponse("Part not found");

             if (existingPart.StockQty >= RequestableStockThreshold)
            {
                return ApiResponse<PartRequestResponseDTO>
                    .ErrorResponse("This part is currently available and cannot be requested");
            }
            }

            var partRequest = new PartRequest
            {
                CustomerId = customer.Id,
                VehicleId = request.VehicleId,
                PartId = request.PartId,

                PartName = existingPart != null
                    ? existingPart.Name
                    : request.PartName!.Trim(),

                Description = request.Description,
                Status = PartRequestStatus.Pending,
                CreatedAt = DateTimeHelper.UtcNow,
                UpdatedAt = DateTimeHelper.UtcNow
            };

            await _partRequestRepository.AddAsync(partRequest);
            await _partRequestRepository.SaveChangesAsync();

            return await GetByIdAsync(partRequest.Id);
        }

        public async Task<ApiResponse<List<PartRequestResponseDTO>>> GetMyRequestsAsync(string userId)
        {
            var customer = await _customerProfileRepository.GetByUserIdWithVehiclesAsync(userId);

            if (customer == null)
                return ApiResponse<List<PartRequestResponseDTO>>.ErrorResponse("Customer profile not found");

            var requests = await _partRequestRepository.GetMyRequestsAsync(customer.Id);

            var response = requests.Select(MapToResponse).ToList();

            return ApiResponse<List<PartRequestResponseDTO>>
                .SuccessResponse(response, "Part requests retrieved successfully");
        }

        public async Task<ApiResponse<PartRequestResponseDTO>> GetMyRequestByIdAsync(
            string userId,
            Guid requestId)
        {
            var customer = await _customerProfileRepository.GetByUserIdWithVehiclesAsync(userId);

            if (customer == null)
                return ApiResponse<PartRequestResponseDTO>.ErrorResponse("Customer profile not found");

            var request = await _partRequestRepository.GetMyRequestByIdAsync(customer.Id, requestId);

            if (request == null)
                return ApiResponse<PartRequestResponseDTO>.ErrorResponse("Part request not found");

            return ApiResponse<PartRequestResponseDTO>
                .SuccessResponse(MapToResponse(request), "Part request fetched successfully");
        }

        public async Task<ApiResponse<PagedResult<PartRequestResponseDTO>>> GetAllAsync(
            int pageNumber,
            int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var (requests, totalCount) = await _partRequestRepository.GetPagedAsync(pageNumber, pageSize);

            var response = requests.Select(MapToResponse).ToList();

            var paged = PagedResult<PartRequestResponseDTO>.Create(
                response,
                pageNumber,
                pageSize,
                totalCount
            );

            return ApiResponse<PagedResult<PartRequestResponseDTO>>
                .SuccessResponse(paged, "Part requests retrieved successfully");
        }

        public async Task<ApiResponse<PagedResult<PartRequestResponseDTO>>> SearchAsync(
            string? query,
            PartRequestStatus? status,
            int pageNumber,
            int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var (requests, totalCount) = await _partRequestRepository.SearchAsync(
                query,
                status,
                pageNumber,
                pageSize
            );

            var response = requests.Select(MapToResponse).ToList();

            var paged = PagedResult<PartRequestResponseDTO>.Create(
                response,
                pageNumber,
                pageSize,
                totalCount
            );

            return ApiResponse<PagedResult<PartRequestResponseDTO>>
                .SuccessResponse(paged, "Part request search completed");
        }

        public async Task<ApiResponse<PartRequestResponseDTO>> GetByIdAsync(Guid id)
        {
            var request = await _partRequestRepository.GetByIdAsync(id);

            if (request == null)
                return ApiResponse<PartRequestResponseDTO>.ErrorResponse("Part request not found");

            return ApiResponse<PartRequestResponseDTO>
                .SuccessResponse(MapToResponse(request), "Part request fetched successfully");
        }

        public async Task<ApiResponse<PartRequestResponseDTO>> MarkAsSourcedAsync(
            Guid id,
            UpdatePartRequestStatusDTO request)
        {
            var partRequest = await _partRequestRepository.GetByIdAsync(id);

            if (partRequest == null)
                return ApiResponse<PartRequestResponseDTO>.ErrorResponse("Part request not found");

            if (partRequest.Status == PartRequestStatus.Sourced)
                return ApiResponse<PartRequestResponseDTO>.ErrorResponse("Part request is already sourced");

            if (partRequest.Status == PartRequestStatus.Rejected)
                return ApiResponse<PartRequestResponseDTO>.ErrorResponse("Rejected request cannot be sourced");

            partRequest.Status = PartRequestStatus.Sourced;
            partRequest.AdminNote = request.AdminNote;
            partRequest.UpdatedAt = DateTimeHelper.UtcNow;

            _partRequestRepository.Update(partRequest);
            await _partRequestRepository.SaveChangesAsync();

            var customerEmail = partRequest.Customer.User?.Email;
            var customerName = partRequest.Customer.FullName;

            if (!string.IsNullOrWhiteSpace(customerEmail))
            {
                await _emailService.SendPartRequestSourcedEmailAsync(
                    customerEmail,
                    customerName,
                    partRequest.PartName
                );
            }

            return await GetByIdAsync(id);
        }

        public async Task<ApiResponse<PartRequestResponseDTO>> RejectAsync(
            Guid id,
            UpdatePartRequestStatusDTO request)
        {
            var partRequest = await _partRequestRepository.GetByIdAsync(id);

            if (partRequest == null)
                return ApiResponse<PartRequestResponseDTO>.ErrorResponse("Part request not found");

            if (partRequest.Status == PartRequestStatus.Sourced)
                return ApiResponse<PartRequestResponseDTO>.ErrorResponse("Sourced request cannot be rejected");

            if (partRequest.Status == PartRequestStatus.Rejected)
                return ApiResponse<PartRequestResponseDTO>.ErrorResponse("Part request is already rejected");

            partRequest.Status = PartRequestStatus.Rejected;
            partRequest.AdminNote = request.AdminNote;
            partRequest.UpdatedAt = DateTimeHelper.UtcNow;

            _partRequestRepository.Update(partRequest);
            await _partRequestRepository.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        private static PartRequestResponseDTO MapToResponse(PartRequest request)
        {
            return new PartRequestResponseDTO
            {
                Id = request.Id,
                CustomerId = request.CustomerId,
                CustomerName = request.Customer?.FullName ?? "",
                VehicleId = request.VehicleId,
                VehicleNumber = request.Vehicle?.VehicleNumber,
                PartId = request.PartId,
                PartName = request.PartName,
                Description = request.Description,
                Status = request.Status.ToString(),
                AdminNote = request.AdminNote,
                CreatedAt = request.CreatedAt
            };
        }
    }
}