namespace IdentityService.Shared.Entities
{
    public class User: BaseEntity
    {
        public string Email { get; set; }
        public string HashPassword { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<UserClaim> Claims { get; set; } = new HashSet<UserClaim>();

    }
}
