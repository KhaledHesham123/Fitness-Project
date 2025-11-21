namespace WorkoutCatalogService.Shared.Entites
{
    public class Workout : BaseEntity
    {
       
        public string Name { get; set; } = string.Empty;  // like Incline Bench Press
        public string Description { get; set; } = string.Empty;
        public DifficultyLevel DifficultyLevel { get; set; }= DifficultyLevel.Beginner; 
        public MuscleGroup MuscleGroup { get; set; }= MuscleGroup.FullBody;
        public int DurationMinutes { get; set; }


        public Guid SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; } = null!;

        public ICollection<PlanWorkout> PlanWorkout { get; set; } = new HashSet<PlanWorkout>();


    }
}
