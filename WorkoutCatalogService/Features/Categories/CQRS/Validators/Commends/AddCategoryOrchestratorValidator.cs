using FluentValidation;
using WorkoutCatalogService.Features.Categories.CQRS.Orchestratots;
using WorkoutCatalogService.Features.Categories.CQRS.Validators.DTOs;

namespace WorkoutCatalogService.Features.Categories.CQRS.Validators.Commends
{
    public class AddCategoryOrchestratorValidator: AbstractValidator<AddCategoryOrchestrator>
    {
        public AddCategoryOrchestratorValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

            RuleFor(x => x.SubCategories)
                .NotNull().WithMessage("SubCategories cannot be null")
                .Must(list => list.Count > 0).WithMessage("At least one subcategory is required");

            RuleForEach(x => x.SubCategories)
                .SetValidator(new SubCategoryDtoValidator());
        }
    }
}
