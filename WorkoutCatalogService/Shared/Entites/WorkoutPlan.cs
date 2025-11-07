namespace WorkoutCatalogService.Shared.Entites
{
    public class WorkoutPlan:BaseEntity
    {
        // like chest day
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DifficultyLevel DifficultyLevel { get; set; }
        public ICollection<WorkoutExercise> Exercises { get; set; } =new HashSet<WorkoutExercise>();
    }
}
