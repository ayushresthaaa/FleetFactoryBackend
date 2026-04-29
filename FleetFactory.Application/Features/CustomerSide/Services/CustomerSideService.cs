using FleetFactory.Application.Features.CustomerSide.DTOs;
using FleetFactory.Application.Interfaces.Repositories;
using FleetFactory.Application.Interfaces.Services;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Features.CustomerSide.Services
{
    public class CustomerSideService(ICustomerSideRepository _customerSideRepository)
        : ICustomerSideService
    {
        public async Task<ApiResponse<List<CustomerPurchaseHistoryResponseDto>>>
            GetPurchaseHistoryAsync(Guid customerId)
        {
            var invoices = await _customerSideRepository
                .GetPurchaseHistoryAsync(customerId);

            var response = invoices.Select(s => new CustomerPurchaseHistoryResponseDto
            {
                SalesInvoiceId = s.Id,
                InvoiceNo = s.InvoiceNo,
                Status = s.Status,
                TotalAmount = s.TotalAmount,
                CreatedAt = s.CreatedAt
            }).ToList();

            return ApiResponse<List<CustomerPurchaseHistoryResponseDto>>
                .SuccessResponse(response, "Purchase history retrieved successfully");
        }
    }
}