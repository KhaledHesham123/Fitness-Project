using IdentityService.Features.Shared;
using IdentityService.Shared.Entities;
using MediatR;

namespace IdentityService.Features.Authantication.Commands.AddRefreshToken
{
    public record AddRefreshTokenCommand(RefreshToken token): IRequest<Result<bool>>;

}
