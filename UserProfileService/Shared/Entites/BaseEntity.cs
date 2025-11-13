namespace UserProfileService.Shared.Entites
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public bool IsDeleted { get; set; } = false;
    }
}
