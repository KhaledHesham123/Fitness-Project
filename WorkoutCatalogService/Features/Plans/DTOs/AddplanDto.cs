using WorkoutCatalogService.Shared.Entites;

namespace WorkoutCatalogService.Features.Plans.DTOs
{
    public class AddplanDto
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DifficultyLevel DifficultyLevel { get; set; }


        public ICollection<Guid> ExerciseId { get; set; } = new HashSet<Guid>();


    }
}
