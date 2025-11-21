using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Features.Categories.DTOs
{
    public class SubCategoryDTo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

    }
}