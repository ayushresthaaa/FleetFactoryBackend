using FleetFactory.Application.Features.SalesInvoices.DTOs;
using FleetFactory.Shared.Results;

namespace FleetFactory.Application.Interfaces.Services
{
    public interface ISalesInvoiceService
    {
        Task<ApiResponse<PagedResult<SalesInvoiceResponseDto>>> GetAllAsync(int pageNumber, int pageSize);

        Task<ApiResponse<SalesInvoiceResponseDto>> GetByIdAsync(Guid id);

        Task<ApiResponse<SalesInvoiceResponseDto>> CreateAsync(
            CreateSalesInvoiceRequestDto request,
            string createdById);

        Task<ApiResponse<SalesInvoiceResponseDto>> MarkPaidAsync(Guid id);

        Task<ApiResponse<SalesInvoiceResponseDto>> CancelAsync(Guid id);
    }
}