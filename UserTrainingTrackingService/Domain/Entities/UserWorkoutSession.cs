using UserTrainingTrackingService.Domain.Enums;

namespace UserTrainingTrackingService.Domain.Entities
{
    public class UserWorkoutSession : BaseEntity
    {
        public int UserId { get; set; }
        public int WorkoutPlanId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public WorkoutStatus Status { get; set; } = WorkoutStatus.InProgress;
        public List<WorkoutExerciseCompletion> CompletedExercises { get; set; } = new List<WorkoutExerciseCompletion>();
    }
}
