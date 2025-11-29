using FluentValidation;
using WorkoutCatalogService.Features.Categories.CQRS.Commends;
using WorkoutCatalogService.Features.Categories.CQRS.Validators.DTOs;

namespace WorkoutCatalogService.Features.Categories.CQRS.Validators.Commends
{
    public class AddSubcategoriesValidator : AbstractValidator<AddsubcategoriesCommend>
    {
        public AddSubcategoriesValidator()
        {
            RuleFor(x => x.SubCategoryDTos)
                .NotNull().WithMessage("Subcategories list cannot be null.")
                .Must(list => list.Any()).WithMessage("Subcategories list cannot be empty.");

            RuleForEach(x => x.SubCategoryDTos)
                .SetValidator(new SubCategoryDtoValidator());
        }
    }
}
