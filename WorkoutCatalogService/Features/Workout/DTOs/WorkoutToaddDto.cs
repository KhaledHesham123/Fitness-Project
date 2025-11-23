using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Features.Workout.DTOs
{
    public class WorkoutToaddDto
    {
        public Guid id { get; set; }=Guid.NewGuid();

        public string Name { get; set; } = string.Empty;  // like Incline Bench Press
        public string Description { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; } 
        public string MuscleGroup { get; set; } 
        public int DurationMinutes { get; set; }


        public Guid SubCategoryId { get; set; }


    }
}
