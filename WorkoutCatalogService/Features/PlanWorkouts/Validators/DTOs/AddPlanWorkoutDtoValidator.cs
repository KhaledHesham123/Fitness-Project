using FluentValidation;
using WorkoutCatalogService.Features.PlanWorkouts.DTOS;

namespace WorkoutCatalogService.Features.PlanWorkouts.Validators.DTOs
{
    public class AddPlanWorkoutDtoValidator:AbstractValidator<AddPlanWorkoutDto>
    {
        public AddPlanWorkoutDtoValidator()
        {
            RuleFor(x => x.Sets)
               .GreaterThan(0).WithMessage("Sets must be greater than 0.");

            RuleFor(x => x.Reps)
                .GreaterThan(0).WithMessage("Reps must be greater than 0.");

            RuleFor(x => x.WorkoutPlanId)
                .NotEmpty().WithMessage("WorkoutPlanId cannot be an empty Guid.");

            RuleFor(x => x.ExerciseId)
                .NotEmpty().WithMessage("ExerciseId cannot be an empty Guid.");
        }
    }
}
