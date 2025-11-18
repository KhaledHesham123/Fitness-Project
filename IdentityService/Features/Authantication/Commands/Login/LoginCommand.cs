using IdentityService.Features.Shared;
using MediatR;

namespace IdentityService.Features.Authantication.Commands.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<Result<AuthModel>>;
}
