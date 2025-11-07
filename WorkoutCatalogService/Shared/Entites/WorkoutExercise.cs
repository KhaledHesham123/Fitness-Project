namespace WorkoutCatalogService.Shared.Entites
{
    public class WorkoutExercise:BaseEntity
    {
        public int Sets { get; set; }
        public int Reps { get; set; }


        public Guid WorkoutPlanId { get; set; }
        public WorkoutPlan WorkoutPlan { get; set; } = null!;

        public Guid ExerciseId { get; set; }
        public Exercise Exercise { get; set; } = null!;
    }
}
