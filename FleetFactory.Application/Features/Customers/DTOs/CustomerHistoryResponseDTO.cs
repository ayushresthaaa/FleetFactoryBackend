using FleetFactory.Application.Features.Customers.DTOs;

namespace FleetFactory.Application.Features.Customers.DTOs
{
    public class CustomerHistoryResponseDTO
    {
        public Guid CustomerId { get; set; }

        public string UserId { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public decimal CreditBalance { get; set; }

        public List<VehicleResponseDto> Vehicles { get; set; } = new();

        public List<CustomerSalesInvoiceHistoryDTO> PurchaseHistory { get; set; } = new();
    }
}