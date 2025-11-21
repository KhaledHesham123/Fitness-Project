using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Features.Categories.DTOs
{
    public class CategoryToaddDTO
    {
        public Guid id { get; set; }= Guid.NewGuid();

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<SubCategoryDTo> SubCategories { get; set; } = new HashSet<SubCategoryDTo>();
    }
}
