namespace WorkoutCatalogService.Shared.Entites
{
    public class SubCategory:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Guid CategoryId { get; set; }
        public category Category { get; set; } = null!;

        public ICollection<Workout> Workout { get; set; } = new HashSet<Workout>();


    }
}
