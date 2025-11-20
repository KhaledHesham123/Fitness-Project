
using Exam_System.Shared.Services;
using IdentityService.Features.Authantication.CreateUserToken;
using IdentityService.Features.Shared;
using IdentityService.Features.Shared.Queries.GetByCriteria;
using IdentityService.Shared.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityService.Features.Authantication.ForgotPassword
{
    public class ForgotPasswordCommandOrchestrator : IRequestHandler<ForgotPasswordCommand, Result<ForgotPasswordDTO>>
    {
        private readonly IMediator _mediator;
        private readonly EmailVerificationService _emailService;
        private readonly IMemoryCache _memoryCache;

        public ForgotPasswordCommandOrchestrator(IMediator mediator, EmailVerificationService emailService, IMemoryCache memoryCache)
        {
            _mediator = mediator;
            _emailService = emailService;
            _memoryCache = memoryCache;
        }

        public async Task<Result<ForgotPasswordDTO>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var userResult = await _mediator.Send(new GetByCriteriaQuery<User, Guid>
            {
                Criteria = u => u.Email == request.Email,
                Selector = u => u.Id
            });

            if (userResult is null)
                return Result<ForgotPasswordDTO>.FailResponse("email doesn't exist", errors: ["email doesn't exist"], 404);

            var userId = userResult.Data;
            var code = _emailService.GenerateVerificationCode();
            int epirationInMinutes = 1;
            
            await _mediator.Send(new CreateUserTokenCommand(userId, code, epirationInMinutes));

            await _emailService.SendVerificationEmailAsync(request.Email, code);

            _memoryCache.Set(code, userId, TimeSpan.FromMinutes(epirationInMinutes));

            return Result<ForgotPasswordDTO>.SuccessResponse(new ForgotPasswordDTO
            {
                Email = request.Email,
                ExpirationInMinutes = epirationInMinutes
            }, message: "Verification code sent to your email", 201);

        }
    }
}
