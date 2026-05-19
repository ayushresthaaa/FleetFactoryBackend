namespace FleetFactory.Application.Features.Reports.DTOs
{
    public class ReportRowDTO
    {
        public string Name { get; set; } = null!;

        public decimal Value { get; set; }

        public int Count { get; set; }
    }
}