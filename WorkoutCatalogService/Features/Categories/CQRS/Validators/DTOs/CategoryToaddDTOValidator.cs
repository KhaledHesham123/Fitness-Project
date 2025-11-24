using FluentValidation;
using WorkoutCatalogService.Features.Categories.DTOs;

namespace WorkoutCatalogService.Features.Categories.CQRS.Validators.DTOs
{
    public class CategoryToaddDTOValidator : AbstractValidator<CategoryToaddDTO>
    {
        public CategoryToaddDTOValidator()
        {
            // Name validation
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");

            // Description validation
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Category description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            // SubCategories validation
            RuleFor(x => x.SubCategories)
                .NotNull().WithMessage("SubCategories collection cannot be null.")
                .Must(list => list.Any()).WithMessage("At least one subcategory is required.");

            // Apply validator to each SubCategory
            RuleForEach(x => x.SubCategories)
                .SetValidator(new SubCategoryDtoValidator());
        }
    }
}
