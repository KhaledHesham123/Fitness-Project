namespace UserTrainingTrackingService.Domain.Entities
{
    public class WorkoutExerciseCompletion : BaseEntity
    {
        public int ExerciseId { get; set; }
        public int UserWorkoutSessionId { get; set; }
        public UserWorkoutSession UserWorkoutSession { get; set; }
        public bool IsCompleted { get; set; }
    }
}
