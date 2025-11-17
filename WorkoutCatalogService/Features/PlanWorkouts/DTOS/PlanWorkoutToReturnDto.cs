namespace WorkoutCatalogService.Features.PlanWorkouts.DTOS
{
    public class PlanWorkoutToReturnDto
    {
        public Guid Id { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }

        public string WorkoutName { get; set; }
    }
}
