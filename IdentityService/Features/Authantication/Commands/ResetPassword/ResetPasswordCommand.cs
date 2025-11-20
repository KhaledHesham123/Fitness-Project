using IdentityService.Features.Shared;
using MediatR;

namespace IdentityService.Features.Authantication.ResetPassword
{
    public record ResetPasswordCommand(string PasswordToken, string NewPassword, string ConfirmPassword) : IRequest<Result<string>>;
}
