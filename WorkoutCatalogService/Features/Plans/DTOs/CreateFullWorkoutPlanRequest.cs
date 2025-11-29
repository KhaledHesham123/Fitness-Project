using WorkoutCatalogService.Features.PlanWorkouts.DTOS;
using WorkoutCatalogService.Features.Workout.DTOs;

namespace WorkoutCatalogService.Features.Plans.DTOs
{
    public class CreateFullWorkoutPlanRequest
    {
        public AddplanDto Plan { get; set; }
        public IEnumerable<AddPlanWorkoutDto> PlanWorkouts { get; set; }
        public IEnumerable<WorkoutToaddDto> Workouts { get; set; }
    }
}
