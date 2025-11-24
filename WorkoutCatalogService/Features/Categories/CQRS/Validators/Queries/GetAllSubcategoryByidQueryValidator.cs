using FluentValidation;
using WorkoutCatalogService.Features.Categories.CQRS.Quries;

namespace WorkoutCatalogService.Features.Categories.CQRS.Validators.Queries
{
    public class GetAllSubcategoryByidQueryValidator: AbstractValidator<GetAllSubcategoryByidQuery>
    {
        public GetAllSubcategoryByidQueryValidator()
        {
            RuleFor(x => x.Subcategoryid)
                .NotNull().WithMessage("Subcategoryid collection cannot be null.")
                .Must(list => list.Any()).WithMessage("At least one subcategory id is required.");

            RuleForEach(x => x.Subcategoryid)
                .NotEmpty().WithMessage("Subcategory id cannot be empty GUID.");
        }
    }
}
