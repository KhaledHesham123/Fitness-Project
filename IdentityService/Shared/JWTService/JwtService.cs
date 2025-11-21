using IdentityService.Shared.Entities;

namespace IdentityService.Shared.JWTService
{
    public class JwtService : IJwtService
    {
        public Task<string> CreateToken(User user, IList<UserClaim> claims)
        {
            throw new NotImplementedException();
        }
    }
}
