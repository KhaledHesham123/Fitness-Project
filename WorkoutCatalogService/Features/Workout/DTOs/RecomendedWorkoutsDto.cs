using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Features.Workout.DTOs
{
    public class RecomendedWorkoutsDto
    {

        public string Name { get; set; } = string.Empty;  
        public string Description { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; } 
        public int DurationMinutes { get; set; }


        public string SubCategoryName { get; set; } = null!;

    }
}
