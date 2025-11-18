using IdentityService.Features.Shared;
using MediatR;

namespace IdentityService.Features.Authantication.Commands.Register
{
    public record RegisterCommand(
    string Email,
    string Password,
    string PhoneNumber) : IRequest<Result<string>>;
}
