using IdentityService.Features.Shared;
using IdentityService.Shared.Interfaces;
using MediatR;

namespace IdentityService.Features.Authantication.Commands.TokenRefresh
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthModel>>
    {
        private readonly IAuthService _authService;

        public RefreshTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }
        public async Task<Result<AuthModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokens = await _authService.RefreshTokenAsync(request.refreshToken);
            return tokens.IsAuthenticated ?
                Result<AuthModel>.SuccessResponse(tokens, "Token refreshed successfully", 200) :
                Result<AuthModel>.FailResponse("Invalid or expired refresh token", errors: ["Invalid or expired refresh token"], 401);
        }
    }
}
