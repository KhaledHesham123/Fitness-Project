using FluentValidation;
using WorkoutCatalogService.Features.Plans.CQRS.Commends;
using WorkoutCatalogService.Features.Plans.Validators.DTOs;

namespace WorkoutCatalogService.Features.Plans.Validators.Commends
{
    public class AddPlanCommendValidator:AbstractValidator<AddPlanCommend>
    {
        public AddPlanCommendValidator()
        {
            RuleFor(x => x.AddplanDto)
                .NotNull().WithMessage("PlanDTO cannot be null.")
                .SetValidator(new AddplanDtoValidator());

        }
    }
}
