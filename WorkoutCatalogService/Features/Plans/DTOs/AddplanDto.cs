using System.ComponentModel.DataAnnotations;
using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Features.Plans.DTOs
{
    public class AddplanDto
    {
        public Guid id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;
        public DifficultyLevel DifficultyLevel { get; set; }

        public Guid AssignedUserIds {  get; set; }

        public ICollection<Guid> ExerciseId { get; set; } = new HashSet<Guid>();


    }
}
