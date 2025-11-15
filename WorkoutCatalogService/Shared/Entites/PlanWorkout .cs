namespace WorkoutCatalogService.Shared.Entites
{
    public class PlanWorkout : BaseEntity
    {
        public int Sets { get; set; }
        public int Reps { get; set; }


        public Guid WorkoutPlanId { get; set; }
        public Plan WorkoutPlan { get; set; } = null!;

        public Guid ExerciseId { get; set; }
        public Workout Workout { get; set; } = null!;
    }
}
