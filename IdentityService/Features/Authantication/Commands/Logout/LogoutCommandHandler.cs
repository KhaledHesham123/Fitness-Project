using IdentityService.Features.Shared;
using IdentityService.Shared.Entities;
using IdentityService.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Authantication.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<string>>
    {
        private readonly IRepository<IdentityService.Shared.Entities.RefreshToken> _refreshTokenRepository;

        public LogoutCommandHandler(IRepository<RefreshToken> refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Result<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var hasActiveToken = await _refreshTokenRepository
                   .GetAll()
                   .AnyAsync(rt => rt.UserId == request.userId && rt.RevokedOn == null, cancellationToken);

            if (!hasActiveToken)
            {
                return Result<string>.FailResponse(
                    "No active session found",
                    errors: ["No active session found"],
                    statusCode: 404
                );
            }
            var revokedToken = await _refreshTokenRepository.GetAll()
                .Where(rt => rt.UserId == request.userId)
                .ExecuteUpdateAsync(rt => rt.SetProperty(p => p.RevokedOn, DateTime.UtcNow));

            return Result<string>.SuccessResponse(null, "Logged out successfully", 200);
        }
    }
}
