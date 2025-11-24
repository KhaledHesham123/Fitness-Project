using System.ComponentModel.DataAnnotations;
using WorkoutCatalogService.Features.PlanWorkouts.DTOS;
using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Features.Plans.DTOs
{
    public class PalnToReturnDto
    {
        public Guid id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; }
        public ICollection<PlanWorkoutToReturnDto> PlanWorkout { get; set; } = new HashSet<PlanWorkoutToReturnDto>();

        public ICollection<Guid> AssignedUserIds { get; set; } = new HashSet<Guid>();

        public IEnumerable<string> UserName { get; set; }
    }
}
