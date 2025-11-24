using FluentValidation;
using WorkoutCatalogService.Features.Categories.DTOs;

namespace WorkoutCatalogService.Features.Categories.CQRS.Validators.DTOs
{
    public class SubCategoryDtoValidator : AbstractValidator<SubCategoryDTo>
    {
        public SubCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("SubCategory name is required")
                .MaximumLength(100).WithMessage("SubCategory name must not exceed 100 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("SubCategory description is required")
                .MaximumLength(500).WithMessage("SubCategory description must not exceed 500 characters");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required");
        }
    }
}
