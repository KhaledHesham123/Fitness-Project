using IdentityService.Features.Shared;
using MediatR;

namespace IdentityService.Features.Authantication.Commands.Logout
{
    public record LogoutCommand(Guid userId) : IRequest<Result<string>>;
}
