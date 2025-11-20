using MediatR;

namespace IdentityService.Features.Authantication.ChangePassword
{
    public record ChangePasswordCommand(Guid UserId, string NewPassword) : IRequest;
}
