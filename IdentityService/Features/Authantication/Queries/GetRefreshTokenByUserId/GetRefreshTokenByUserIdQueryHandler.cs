using IdentityService.Features.Shared;
using IdentityService.Shared.Entities;
using IdentityService.Shared.Interfaces;
using MediatR;

namespace IdentityService.Features.Authantication.Queries.GetRefreshTokenByUserId
{
    public class GetRefreshTokenByUserIdQueryHandler : IRequestHandler<GetRefreshTokenByUserIdQuery, Result<RefreshToken>>
    {
        private readonly IRepository<RefreshToken> _refreshTokenRepository;

        public GetRefreshTokenByUserIdQueryHandler(IRepository<RefreshToken> refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<Result<RefreshToken>> Handle(GetRefreshTokenByUserIdQuery request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.FirstOrDefaultAsync(rt => rt.UserId == request.userId && rt.IsActive);

            return refreshToken is not null ?
                Result<RefreshToken>.SuccessResponse(refreshToken, "refresh token retrieved successfully") :
                Result<RefreshToken>.FailResponse("Refresh token not found");
        }
    }
}
