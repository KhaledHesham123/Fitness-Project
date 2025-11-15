namespace WorkoutCatalogService.Shared.Entites
{
    public class Plan : BaseEntity
    {
        // like chest day
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DifficultyLevel DifficultyLevel { get; set; }
        public ICollection<PlanWorkout> PlanWorkout { get; set; } =new HashSet<PlanWorkout>();
    }
}
