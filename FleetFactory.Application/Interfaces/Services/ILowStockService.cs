using FleetFactory.Application.Features.LowStock.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface ILowStockService
    {
        Task<ApiResponse<List<LowStockNotificationResponseDTO>>> 
            CheckLowStockAsync(int threshold = 10);
    }
}