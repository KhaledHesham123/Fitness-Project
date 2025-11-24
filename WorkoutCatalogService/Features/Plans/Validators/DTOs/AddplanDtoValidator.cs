using FluentValidation;
using WorkoutCatalogService.Features.Plans.DTOs;

namespace WorkoutCatalogService.Features.Plans.Validators.DTOs
{
    public class AddplanDtoValidator:AbstractValidator<AddplanDto>
    {
        public AddplanDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Plan name is required.")
                .MaximumLength(100).WithMessage("Plan name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.DifficultyLevel)
                .NotEmpty().WithMessage("Difficulty level is required.")
                .Must(level => level == "Easy" || level == "Medium" || level == "Hard")
                .WithMessage("DifficultyLevel must be one of: Easy, Medium, Hard.");

            RuleFor(x => x.AssignedUserIds)
                .NotNull().WithMessage("AssignedUserIds cannot be null.")
                .Must(list => list.Any()).WithMessage("At least one user Id must be assigned.");

            RuleForEach(x => x.AssignedUserIds)
                .NotEmpty().WithMessage("User Id cannot be an empty Guid.");
        }
    }
}
