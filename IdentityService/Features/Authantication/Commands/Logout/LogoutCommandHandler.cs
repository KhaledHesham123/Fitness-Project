using IdentityService.Features.Shared;
using IdentityService.Shared.Entities;
using IdentityService.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Authantication.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<string>>
    {
        private readonly IRepository<RefreshToken> _refreshTokenRepository;

        public LogoutCommandHandler(IRepository<RefreshToken> refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Result<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var revokedToken = await _refreshTokenRepository.GetAll()
                .Where(rt => rt.UserId == request.userId)
                .ExecuteUpdateAsync(rt => rt.SetProperty(p => p.RevokedOn, DateTime.UtcNow));

            return revokedToken > 0 ?
                Result<string>.SuccessResponse("Logged out successfully", "Logged out successfully", 200) :
                Result<string>.FailResponse("No active session found", errors: ["No active session found"], 404);
        }
    }
}
