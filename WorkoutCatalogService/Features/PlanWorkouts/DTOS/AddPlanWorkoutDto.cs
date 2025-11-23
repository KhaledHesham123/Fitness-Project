using System.ComponentModel.DataAnnotations;
using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Features.PlanWorkouts.DTOS
{
    public class AddPlanWorkoutDto
    {
        public Guid Id { get; set; }= Guid.NewGuid();
        [Range(1, int.MaxValue, ErrorMessage = "Sets must be at least 1.")]

        public int Sets { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Reps must be at least 1.")]

        public int Reps { get; set; }


        public Guid WorkoutPlanId { get; set; }

        public Guid ExerciseId { get; set; }
    }
}
