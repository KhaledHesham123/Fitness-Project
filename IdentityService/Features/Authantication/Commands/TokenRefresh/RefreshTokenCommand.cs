using IdentityService.Features.Shared;
using MediatR;

namespace IdentityService.Features.Authantication.Commands.TokenRefresh
{
    public record RefreshTokenCommand(string refreshToken) : IRequest<Result<AuthModel>>;
}
