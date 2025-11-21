using IdentityService.Shared.Entities;

namespace IdentityService.Shared.JWTService
{
    public interface IJwtService
    {
        Task<string> CreateToken(User user , IList<UserClaim> claims);
    }
}
