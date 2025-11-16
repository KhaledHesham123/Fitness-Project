using WorkoutCatalogService.Features.PlanWorkouts.DTOS;
using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Features.Plans.DTOs
{
    public class PalnToReturnDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DifficultyLevel DifficultyLevel { get; set; }
        public ICollection<PlanWorkoutToReturnDto> PlanWorkout { get; set; } = new HashSet<PlanWorkoutToReturnDto>();

        public ICollection<Guid> AssignedUserIds { get; set; } = new HashSet<Guid>();
    }
}
