namespace IdentityService.Shared.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string HashPassword { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        public ICollection<UserPermission> UserPermissions { get; set; } = new HashSet<UserPermission>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();


    }
}
