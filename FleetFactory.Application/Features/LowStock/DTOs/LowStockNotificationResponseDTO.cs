namespace FleetFactory.Application.Features.LowStock.DTOs
{
    public class LowStockNotificationResponseDTO
    {
        public Guid PartId { get; set; }

        public string Sku { get; set; } = null!;

        public string PartName { get; set; } = null!;

        public int StockQty { get; set; }

        public string Message { get; set; } = null!;
    }
}