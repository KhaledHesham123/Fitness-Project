using IdentityService.Features.Authantication;
using IdentityService.Shared.Entities;
using System.Threading.Tasks;

namespace IdentityService.Shared.Interfaces
{
    public interface IAuthService
    {
        Task<AuthModel> GenerateTokensAsync(User user);
        Task<AuthModel> RefreshTokenAsync(string refreshToken);
        Task<string> GeneratePasswordHashAsync(string realPassword);
        Task<bool> CheckPasswordAsync(User user, string password);

    }
}
