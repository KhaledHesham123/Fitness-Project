
namespace IdentityService.Shared.Entities
{
    public class UserClaim: BaseEntity
    {
        public UserClaim() { }
        public UserClaim(User user, string claimType, string claimValue)
        {
            User = user;
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

    }
}
