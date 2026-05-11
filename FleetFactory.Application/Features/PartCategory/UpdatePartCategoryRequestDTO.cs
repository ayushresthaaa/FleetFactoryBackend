namespace FleetFactory.Application.Features.PartCategories.DTOs
{
    public class UpdatePartCategoryRequestDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}