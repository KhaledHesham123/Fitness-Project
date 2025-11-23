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
        public string DifficultyLevel { get; set; }

        public IEnumerable<Guid> AssignedUserIds { get; set; } 






    }
}
