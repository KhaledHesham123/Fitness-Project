using IdentityService.Features.Authantication;
using IdentityService.Shared.Entities;

namespace IdentityService.Shared.JWTService
{
    public interface IJwtService
    {
        Task<AuthModel> GenerateAccessTokenAsync(User user);
        RefreshToken GenerateRefreshToken();
    }
}
