using System.ComponentModel.DataAnnotations;
using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Features.Categories.DTOs
{
    public class CategoriesDTO
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "SubCategories collection is required.")]
        public ICollection<SubCategoryDTo> SubCategories { get; set; } = new HashSet<SubCategoryDTo>();
    }
}
