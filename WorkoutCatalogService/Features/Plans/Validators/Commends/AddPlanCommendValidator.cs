using FluentValidation;
using WorkoutCatalogService.Features.Plans.CQRS.Commends;
using WorkoutCatalogService.Features.Plans.Validators.DTOs;

namespace WorkoutCatalogService.Features.Plans.Validators.Commends
{
    public class AddPlanCommendValidator:AbstractValidator<AddPlanCommend>
    {
        public AddPlanCommendValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Plan name is required")
                .MaximumLength(100).WithMessage("Plan name must not exceed 100 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

            RuleFor(x => x.DifficultyLevel)
                .NotEmpty().WithMessage("Difficulty level is required")
                .Must(dl => Enum.TryParse<WorkoutCatalogService.Shared.Entites.DifficultyLevel>(dl, true, out _))
                .WithMessage("Invalid difficulty level");

            RuleFor(x => x.AssignedUserIds)
                .NotNull().WithMessage("AssignedUserIds cannot be null")
                .Must(ids => ids.Any()).WithMessage("At least one user must be assigned");

        }
    }
}
