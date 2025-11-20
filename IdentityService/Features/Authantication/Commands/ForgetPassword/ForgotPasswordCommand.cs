using IdentityService.Features.Shared;
using MediatR;

namespace IdentityService.Features.Authantication.ForgotPassword
{
    public record ForgotPasswordCommand(string Email) : IRequest<Result<ForgotPasswordDTO>>;
}
