namespace WorkoutCatalogService.Features.Categories.DTOs
{
    public class AddCategoryWithSubcategoriesRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<SubCategoryDTo> SubCategories { get; set; } = new List<SubCategoryDTo>();
    }
}
