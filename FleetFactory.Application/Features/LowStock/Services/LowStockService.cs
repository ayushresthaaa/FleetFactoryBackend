using FleetFactory.Application.Features.LowStock.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Domain.Entities;
using FleetFactory.Infrastructure.Helpers;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Features.LowStock.Services
{
    public class LowStockService(ILowStockRepository _lowStockRepository) : ILowStockService
    {
        public async Task<ApiResponse<List<LowStockNotificationResponseDTO>>> 
            CheckLowStockAsync(int threshold = 10)
        {
            var lowStockParts = await _lowStockRepository.GetLowStockPartsAsync(threshold);

            var response = new List<LowStockNotificationResponseDTO>();

            foreach (var part in lowStockParts)
            {
                var message = $"{part.Name} is low in stock. Current quantity: {part.StockQty}";

                var notification = new Notification
                {
                    Type = "low_stock",
                    Title = "Low Stock Alert",
                    Message = message,
                    ReferenceId = part.Id,
                    IsRead = false,
                    CreatedAt = DateTimeHelper.UtcNow
                };

                await _lowStockRepository.AddNotificationAsync(notification);

                response.Add(new LowStockNotificationResponseDTO
                {
                    PartId = part.Id,
                    Sku = part.Sku,
                    PartName = part.Name,
                    StockQty = part.StockQty,
                    Message = message
                });
            }

            await _lowStockRepository.SaveChangesAsync();

            return ApiResponse<List<LowStockNotificationResponseDTO>>
                .SuccessResponse(response, "Low stock check completed");
        }
    }
}