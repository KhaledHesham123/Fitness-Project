using FluentValidation;
using WorkoutCatalogService.Features.Workout.DTOs;

namespace WorkoutCatalogService.Features.Workout.Validators.DTOs
{
    public class WorkoutToaddDtoValidator:AbstractValidator<WorkoutToaddDto>
    {
        public WorkoutToaddDtoValidator()
        {
            RuleFor(x => x.Name)
              .NotEmpty().WithMessage("Workout name is required.")
              .MaximumLength(100).WithMessage("Workout name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.DifficultyLevel)
                .NotEmpty().WithMessage("Difficulty level is required.")
                .Must(level => level == "Easy" || level == "Medium" || level == "Hard")
                .WithMessage("DifficultyLevel must be one of: Easy, Medium, Hard.");

            RuleFor(x => x.MuscleGroup)
                .NotEmpty().WithMessage("MuscleGroup is required.")
                .MaximumLength(100).WithMessage("MuscleGroup must not exceed 100 characters.");

            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0).WithMessage("DurationMinutes must be greater than 0.")
                .LessThanOrEqualTo(300).WithMessage("DurationMinutes cannot exceed 300 minutes.");

            RuleFor(x => x.SubCategoryId)
                .NotEmpty().WithMessage("SubCategoryId cannot be an empty Guid.");
        }
    }
}
