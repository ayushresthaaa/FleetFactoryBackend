namespace FleetFactory.Application.Features.Reports.DTOs
{
    public class DashboardDTO
    {
        public List<SummaryCardDTO> Cards { get; set; } = new();

        public List<ChartPointDTO> Charts { get; set; } = new();
    }
}