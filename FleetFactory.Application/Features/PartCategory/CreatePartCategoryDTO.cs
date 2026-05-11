namespace FleetFactory.Application.Features.PartCategories.DTOs
{
    public class CreatePartCategoryRequestDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}