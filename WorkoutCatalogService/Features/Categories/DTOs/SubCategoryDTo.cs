using System.ComponentModel.DataAnnotations;
using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Features.Categories.DTOs
{
    public class SubCategoryDTo
    {
        public Guid id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }

    }
}