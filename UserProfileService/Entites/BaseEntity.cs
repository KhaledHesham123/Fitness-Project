namespace UserProfileService.Entity
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsDeleted { get; set; } = false;
        public DateTime UpdatedAt { get; set; }

        public DateTime DeletedAt { get; set; }
    }
}
