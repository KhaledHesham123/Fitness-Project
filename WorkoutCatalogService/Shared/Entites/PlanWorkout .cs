namespace WorkoutCatalogService.Shared.Entites
{
    public class PlanWorkout : BaseEntity
    {
        public int Sets { get; set; }
        public int Reps { get; set; }


        public Guid WorkoutPlanId { get; set; }

        public Guid ExerciseId { get; set; }
    }
}
