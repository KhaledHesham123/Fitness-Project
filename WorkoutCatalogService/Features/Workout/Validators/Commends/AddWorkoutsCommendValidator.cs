using FluentValidation;
using WorkoutCatalogService.Features.Workout.CQRS.Commend;
using WorkoutCatalogService.Features.Workout.Validators.DTOs;

namespace WorkoutCatalogService.Features.Workout.Validators.Commends
{
    public class AddWorkoutsCommendValidator:AbstractValidator<AddWorkoutsCommend>
    {
        public AddWorkoutsCommendValidator()
        {
            RuleFor(RuleFor=> RuleFor.AddWorkoutDto).NotEmpty().WithMessage("WorkoutDtos cannot be empty");

            RuleForEach(x => x.AddWorkoutDto).SetValidator(new WorkoutToaddDtoValidator());

        }
    }
}
