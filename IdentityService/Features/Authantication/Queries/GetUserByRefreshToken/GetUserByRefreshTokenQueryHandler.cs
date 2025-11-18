using IdentityService.Features.Shared;
using IdentityService.Shared.Entities;
using IdentityService.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Authantication.Queries.GetUserByRefreshToken
{
    public class GetUserByRefreshTokenQueryHandler : IRequestHandler<GetUserByRefreshTokenQuery, Result<User>>
    {
        private readonly IRepository<RefreshToken> _refreshTokenRepository;

        public GetUserByRefreshTokenQueryHandler(IRepository<RefreshToken> refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<Result<User>> Handle(GetUserByRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await _refreshTokenRepository.GetAll()
                .Where(rt => rt.Token == request.refreshToken)
                .Select(rt => rt.User).FirstOrDefaultAsync(cancellationToken);

            return user is not null
                ? Result<User>.SuccessResponse(user, "user retrieved successfully")
                : Result<User>.FailResponse("Invalid refresh token");
        }
    }
}
