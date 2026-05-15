using FleetFactory.Application.Features.SalesInvoices.DTOs;
using FleetFactory.Shared.Results;
using FleetFactory.Domain.Enums;
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
        Task<ApiResponse<PagedResult<SalesInvoiceResponseDto>>> SearchAsync(
            string? query,
            InvoiceStatus? status,
            SalesInvoiceMode? mode,
            int pageNumber,
            int pageSize);

        Task<ApiResponse<List<CustomerAppointmentForInvoiceDto>>> GetCustomerAppointmentsAsync(Guid customerId);
    }
}