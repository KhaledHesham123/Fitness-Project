using WorkoutCatalogService.Features.Categories.DTOs;
using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Features.Workout.DTOs
{
    public class WorkoutToreturnDto
    {
        public string Name { get; set; } = string.Empty;  // like Incline Bench Press
        public string Description { get; set; } = string.Empty;
        public DifficultyLevel DifficultyLevel { get; set; } = DifficultyLevel.Beginner;
        public MuscleGroup MuscleGroup { get; set; } = MuscleGroup.FullBody;
        public int DurationMinutes { get; set; }


        public Guid SubCategoryId { get; set; }
        public SubCategoryDTo SubCategory { get; set; } = null!;
    }
}
