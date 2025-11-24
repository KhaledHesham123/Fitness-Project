using FluentValidation;
using WorkoutCatalogService.Features.Categories.CQRS.Commends;

namespace WorkoutCatalogService.Features.Categories.CQRS.Validators.Commends
{
    public class AddCategoryValidator: AbstractValidator<AddCategoryCommend>   
    {
        public AddCategoryValidator() 
        {
       

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required")
            .MaximumLength(100).WithMessage("Category name must not exceed 100 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");
        }
    }
}
