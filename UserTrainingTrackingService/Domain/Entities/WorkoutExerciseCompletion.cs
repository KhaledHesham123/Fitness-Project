namespace UserTrainingTrackingService.Domain.Entities
{
    public class WorkoutExerciseCompletion
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public int UserWorkoutSessionId { get; set; }
        public UserWorkoutSession UserWorkoutSession { get; set; }
        public bool IsCompleted { get; set; }
    }
}
