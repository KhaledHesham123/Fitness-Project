using WorkoutCatalogService.Features.Categories.DTOs;
using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Features.Workout.DTOs
{
    public class WorkoutToreturnDto
    {
        public Guid id { get; set; }
        public string Name { get; set; } = string.Empty;  // like Incline Bench Press
        public string Description { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; } 
        public string MuscleGroup { get; set; } 
        public int DurationMinutes { get; set; }


        public Guid SubCategoryId { get; set; }
        public SubCategoryDTo SubCategory { get; set; } = null!;
    }
}
