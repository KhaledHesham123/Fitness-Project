using Exam_System.Domain.Entities;
using IdentityService.Features.Authantication.ChangePassword;
using IdentityService.Features.Authantication.ForgotPassword;
using IdentityService.Features.Shared;
using IdentityService.Shared.Interfaces;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityService.Features.Authantication.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<string>>
    {
        private readonly IMediator _mediator;
        private readonly IRepository<UserToken> _repository;
        private readonly IMemoryCache _memoryCache;

        public ResetPasswordCommandHandler(IMediator mediator, IRepository<UserToken> repository, IMemoryCache memoryCache)
        {
            _mediator = mediator;
            _repository = repository;
            _memoryCache = memoryCache;
        }
        public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (_memoryCache.Get<ForgotPasswordDTO>(request.PasswordToken) is not null)
            {
                var userToken = await _repository.FirstOrDefaultAsync(ut => ut.Token == request.PasswordToken);
                if (userToken is null || userToken.ExpiredDate < DateTime.UtcNow)
                {
                    return Result<string>.FailResponse("Invalid or expired token.", errors: ["Invalid or expired token."], statusCode: 404);
                }
                
                await _repository.DeleteAsync(userToken.Id);
                _memoryCache.Remove(request.PasswordToken);
                await _mediator.Send(new ChangePasswordCommand(userToken.UserId, request.NewPassword));
                
                return Result<string>.SuccessResponse(null, "Password reset successful.", statusCode: 201);
            }
            return Result<string>.FailResponse("Invalid or expired token.", errors: ["Invalid or expired token."], statusCode: 404);
        }
    }
}
