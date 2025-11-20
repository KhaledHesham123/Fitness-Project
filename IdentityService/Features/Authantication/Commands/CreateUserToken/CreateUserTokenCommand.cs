using MediatR;

namespace IdentityService.Features.Authantication.CreateUserToken
{
    public record CreateUserTokenCommand(Guid UserId, string Token, int expiredInMin = 1) : IRequest
    {
    }
}
