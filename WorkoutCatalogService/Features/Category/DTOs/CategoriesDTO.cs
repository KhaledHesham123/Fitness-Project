using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Features.Category.DTOs
{
    public class CategoriesDTO
    {
        public Guid id {  get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<SubCategoryDTo> SubCategories { get; set; } = new HashSet<SubCategoryDTo>();
    }
}
