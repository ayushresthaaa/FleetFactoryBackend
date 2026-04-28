using FleetFactory.Application.Features.PurchaseInvoices.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface IPurchaseInvoiceService
    {
        // pagination list
        Task<ApiResponse<PagedResult<PurchaseInvoiceResponseDto>>> 
            GetAllAsync(int pageNumber, int pageSize);

        // single invoice
        Task<ApiResponse<PurchaseInvoiceResponseDto>> 
            GetByIdAsync(Guid id);

        // create invoice (main business logic)
        Task<ApiResponse<PurchaseInvoiceResponseDto>> 
            CreateAsync(CreatePurchaseInvoiceRequestDto request, string createdById);

        // change status can be paid, cancelled, etc. but for now we will only implement mark as paid
        Task<ApiResponse<PurchaseInvoiceResponseDto>> 
            StatusUpdateAsync(Guid id);
    }
}