using FleetFactory.Application.Features.Parts.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Shared.Results;
using FleetFactory.Infrastructure.Helpers; 
namespace FleetFactory.Application.Features.Parts.Services
{
    public class PartService(IPartRepository _partRepository) : IPartService
    {
       public async Task<ApiResponse<PagedResult<PartResponseDto>>> GetAllAsync(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var (parts, totalCount) = await _partRepository.GetPagedAsync(pageNumber, pageSize);

            var response = parts.Select(p => new PartResponseDto
            {
                Id = p.Id,
                Sku = p.Sku,
                Name = p.Name,
                Description = p.Description,
                UnitPrice = p.UnitPrice,
                CostPrice = p.CostPrice,
                StockQty = p.StockQty,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                VendorId = p.VendorId,
                VendorName = p.Vendor?.Name,
                CreatedAt = p.CreatedAt
            }).ToList();

            var pagedResult = PagedResult<PartResponseDto>.Create(
                response,
                pageNumber,
                pageSize,
                totalCount
            );

            return ApiResponse<PagedResult<PartResponseDto>>.SuccessResponse(
                pagedResult,
                "Parts retrieved successfully"
            );
        }

        public async Task<ApiResponse<PartResponseDto>> GetByIdAsync(Guid id)
        {
            var part = await _partRepository.GetByIdAsync(id);

            if (part == null)
                return ApiResponse<PartResponseDto>.ErrorResponse("Part not found");
            
            var response = new PartResponseDto
            {
                Id = part.Id,
                Sku = part.Sku,
                Name = part.Name,
                Description = part.Description,
                UnitPrice = part.UnitPrice,
                CostPrice = part.CostPrice,
                StockQty = part.StockQty,
                IsActive = part.IsActive,
                CategoryId = part.CategoryId,
                CategoryName = part.Category?.Name,
                VendorId = part.VendorId,
                VendorName = part.Vendor?.Name,
                CreatedAt = part.CreatedAt
            };

            return ApiResponse<PartResponseDto>.SuccessResponse(response, "Part fetched");
            
        }

        public async Task<ApiResponse<PartResponseDto>> CreateAsync(CreatePartRequestDto request)
        {

            var skuExists = await _partRepository.ExistsBySkuAsync(request.Sku);

            if (skuExists)
                return ApiResponse<PartResponseDto>
                    .ErrorResponse("SKU already exists");
            var part = new Part
            {
                Sku = request.Sku,
                Name = request.Name,
                Description = request.Description,
                UnitPrice = request.UnitPrice,
                CostPrice = request.CostPrice,
                StockQty = request.StockQty,
                CategoryId = request.CategoryId,
                VendorId = request.VendorId,
                IsActive = true,
                CreatedAt = DateTimeHelper.UtcNow,
                UpdatedAt = DateTimeHelper.UtcNow
            };

            await _partRepository.AddAsync(part);
            await _partRepository.SaveChangesAsync();

            var response = new PartResponseDto
            {
                Id = part.Id,
                Sku = part.Sku,
                Name = part.Name,
                Description = part.Description,
                UnitPrice = part.UnitPrice,
                CostPrice = part.CostPrice,
                StockQty = part.StockQty,
                IsActive = part.IsActive,
                CategoryId = part.CategoryId,
                CategoryName = part.Category?.Name,
                VendorId = part.VendorId,
                VendorName = part.Vendor?.Name,
                CreatedAt = part.CreatedAt
            };

            return ApiResponse<PartResponseDto>.SuccessResponse(response, "Part created successfully");
        }

         public async Task<ApiResponse<PartResponseDto>> UpdateAsync(Guid id, UpdatePartRequestDto request)
        {
            var part = await _partRepository.GetByIdAsync(id);

            if (part == null)
                return ApiResponse<PartResponseDto>.ErrorResponse("Part not found");
            var skuExists = await _partRepository
                .ExistsBySkuExceptIdAsync(request.Sku, id);

            if (skuExists)
                return ApiResponse<PartResponseDto>
                    .ErrorResponse("SKU already exists");
                    
            part.Sku = request.Sku;
            part.Name = request.Name;
            part.Description = request.Description;
            part.UnitPrice = request.UnitPrice;
            part.CostPrice = request.CostPrice;
            part.StockQty = request.StockQty;
            part.CategoryId = request.CategoryId;
            part.VendorId = request.VendorId;

            part.IsActive = request.IsActive;
            part.UpdatedAt = DateTimeHelper.UtcNow;

            _partRepository.Update(part);
            await _partRepository.SaveChangesAsync();

            var response = new PartResponseDto
            {
                Id = part.Id,
                Sku = part.Sku,
                Name = part.Name,
                Description = part.Description,
                UnitPrice = part.UnitPrice,
                CostPrice = part.CostPrice,
                StockQty = part.StockQty,
                IsActive = part.IsActive,
                CategoryId = part.CategoryId,
                VendorId = part.VendorId,
                CreatedAt = part.CreatedAt
            };

            return ApiResponse<PartResponseDto>.SuccessResponse(response, "Part updated");
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            var part = await _partRepository.GetByIdAsync(id);

            if (part == null)
                return ApiResponse<string>.ErrorResponse("Part not found");

            _partRepository.Delete(part);
            await _partRepository.SaveChangesAsync();

            return ApiResponse<string>.SuccessResponse("Deleted", "Part deleted");
        }

        public async Task<ApiResponse<List<PartResponseDto>>> GetLowStockAsync(int threshold)
        {
            threshold = threshold < 1 ? 10 : threshold;

            var parts = await _partRepository.GetLowStockAsync(threshold);

            var response = parts.Select(p => new PartResponseDto
            {
                Id = p.Id,
                Sku = p.Sku,
                Name = p.Name,
                Description = p.Description,
                UnitPrice = p.UnitPrice,
                CostPrice = p.CostPrice,
                StockQty = p.StockQty,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                VendorId = p.VendorId,
                VendorName = p.Vendor?.Name,
                CreatedAt = p.CreatedAt
            }).ToList();

            return ApiResponse<List<PartResponseDto>>
                .SuccessResponse(response, "Low stock parts retrieved");
        }


        public async Task<ApiResponse<List<PartResponseDto>>> GetAvailableAsync()
        {
            var parts = await _partRepository.GetAvailableAsync();

            var response = parts.Select(p => new PartResponseDto
            {
                Id = p.Id,
                Sku = p.Sku,
                Name = p.Name,
                Description = p.Description,
                UnitPrice = p.UnitPrice,
                CostPrice = p.CostPrice,
                StockQty = p.StockQty,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                VendorId = p.VendorId,
                VendorName = p.Vendor?.Name,
                CreatedAt = p.CreatedAt
            }).ToList();

            return ApiResponse<List<PartResponseDto>>
                .SuccessResponse(response, "Available parts retrieved");
        }

        public async Task<ApiResponse<List<StockMovementResponseDto>>> GetStockMovementsAsync(Guid partId)
        {
            var part = await _partRepository.GetByIdAsync(partId);

            if (part == null)
                return ApiResponse<List<StockMovementResponseDto>>
                    .ErrorResponse("Part not found");

            var movements = await _partRepository.GetStockMovementsAsync(partId);

            var response = movements.Select(m => new StockMovementResponseDto
            {
                Id = m.Id,
                PartId = m.PartId,
                PartName = m.Part?.Name,
                MovementType = m.MovementType,
                Quantity = m.Quantity,
                ReferenceId = m.ReferenceId,
                Note = m.Note,
                CreatedById = m.CreatedById,
                CreatedAt = m.CreatedAt
            }).ToList();

            return ApiResponse<List<StockMovementResponseDto>>
                .SuccessResponse(response, "Stock movements retrieved");
        }

        public async Task<ApiResponse<PagedResult<PartResponseDto>>> SearchAsync(
            string keyword,
            int pageNumber,
            int pageSize)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return ApiResponse<PagedResult<PartResponseDto>>
                    .ErrorResponse("Keyword is required");

            var (parts, totalCount) = await _partRepository.SearchAsync(
                keyword,
                pageNumber,
                pageSize);

            var response = parts.Select(p => new PartResponseDto
            {
                Id = p.Id,
                Sku = p.Sku,
                Name = p.Name,
                Description = p.Description,
                UnitPrice = p.UnitPrice,
                CostPrice = p.CostPrice,
                StockQty = p.StockQty,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                VendorId = p.VendorId,
                VendorName = p.Vendor?.Name,
                CreatedAt = p.CreatedAt
            }).ToList();

            var pagedResult = PagedResult<PartResponseDto>.Create(
                response,
                pageNumber,
                pageSize,
                totalCount
            );

            return ApiResponse<PagedResult<PartResponseDto>>
                .SuccessResponse(pagedResult, "Search successful");
        }
    }
}