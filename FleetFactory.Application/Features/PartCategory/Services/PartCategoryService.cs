using FleetFactory.Application.Features.PartCategories.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Helpers;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Features.PartCategories.Services
{
    public class PartCategoryService(
        IPartCategoryRepository _categoryRepository
    ) : IPartCategoryService
    {
        public async Task<ApiResponse<List<PartCategoryResponseDto>>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            var response = categories.Select(c => new PartCategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt
            }).ToList();

            return ApiResponse<List<PartCategoryResponseDto>>
                .SuccessResponse(response, "Categories retrieved successfully");
        }

        public async Task<ApiResponse<PartCategoryResponseDto>> GetByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                return ApiResponse<PartCategoryResponseDto>.ErrorResponse("Category not found");

            var response = new PartCategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt
            };

            return ApiResponse<PartCategoryResponseDto>
                .SuccessResponse(response, "Category fetched successfully");
        }

        public async Task<ApiResponse<PartCategoryResponseDto>> CreateAsync(CreatePartCategoryRequestDto request)
        {
            var exists = await _categoryRepository.ExistsByNameAsync(request.Name);

            if (exists)
                return ApiResponse<PartCategoryResponseDto>.ErrorResponse("Category name already exists");

            var category = new PartCategory
            {
                Name = request.Name.Trim(),
                Description = request.Description,
                IsActive = true,
                CreatedAt = DateTimeHelper.UtcNow,
                UpdatedAt = DateTimeHelper.UtcNow
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            var response = new PartCategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt
            };

            return ApiResponse<PartCategoryResponseDto>
                .SuccessResponse(response, "Category created successfully");
        }

        public async Task<ApiResponse<PartCategoryResponseDto>> UpdateAsync(Guid id, UpdatePartCategoryRequestDto request)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                return ApiResponse<PartCategoryResponseDto>.ErrorResponse("Category not found");

            var exists = await _categoryRepository.ExistsByNameExceptIdAsync(request.Name, id);

            if (exists)
                return ApiResponse<PartCategoryResponseDto>.ErrorResponse("Category name already exists");

            category.Name = request.Name.Trim();
            category.Description = request.Description;
            category.IsActive = request.IsActive;
            category.UpdatedAt = DateTimeHelper.UtcNow;

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            var response = new PartCategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt
            };

            return ApiResponse<PartCategoryResponseDto>
                .SuccessResponse(response, "Category updated successfully");
        }

        public async Task<ApiResponse<string>> DeleteAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                return ApiResponse<string>.ErrorResponse("Category not found");

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync();

            return ApiResponse<string>.SuccessResponse("Deleted", "Category deleted successfully");
        }
    }
}