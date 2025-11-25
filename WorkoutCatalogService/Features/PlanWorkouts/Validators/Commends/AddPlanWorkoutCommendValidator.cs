using FluentValidation;
using WorkoutCatalogService.Features.PlanWorkouts.CQRS.Commends;
using WorkoutCatalogService.Features.PlanWorkouts.Validators.DTOs;

namespace WorkoutCatalogService.Features.PlanWorkouts.Validators.Commends
{
    public class AddPlanWorkoutCommendValidator: AbstractValidator<AddPlanWorkoutCommend>
    {
        public AddPlanWorkoutCommendValidator()
        {
            RuleFor(x=>x.PlanWorkoutDtos).NotNull().WithMessage("PlanWorkoutDtos cannot be null.")
                .Must(List=> List.Any()).WithMessage("PlanWorkoutDtos must contain at least one item.");

            RuleForEach(x => x.PlanWorkoutDtos)
                .SetValidator(new AddPlanWorkoutDtoValidator());

        }
    }
}
