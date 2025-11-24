using System.ComponentModel.DataAnnotations;
using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Features.PlanWorkouts.DTOS
{
    public class AddPlanWorkoutDto
    {
        public Guid Id { get; set; }= Guid.NewGuid();

        public int Sets { get; set; }

        public int Reps { get; set; }


        public Guid WorkoutPlanId { get; set; }

        public Guid ExerciseId { get; set; }
    }
}
