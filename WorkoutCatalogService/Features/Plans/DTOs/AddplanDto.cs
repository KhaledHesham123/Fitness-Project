using System.ComponentModel.DataAnnotations;
using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Features.Plans.DTOs
{
    public class AddplanDto
    {

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string DifficultyLevel { get; set; }

        public IEnumerable<Guid> AssignedUserIds { get; set; } 






    }
}
