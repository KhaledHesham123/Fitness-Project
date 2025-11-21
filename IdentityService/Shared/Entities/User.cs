namespace IdentityService.Shared.Entities
{
    public class User: BaseEntity
    {
        public string Email { get; set; }
        public string HashPassword { get; set; }

        public ICollection<UserClaim> Claims { get; set; } = new HashSet<UserClaim>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

    }
}
