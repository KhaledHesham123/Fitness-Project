using IdentityService.Features.Shared;
using IdentityService.Shared.Entities;
using MediatR;

namespace IdentityService.Features.Authantication.Queries.GetRefreshTokenByUserId
{
    public record GetRefreshTokenByUserIdQuery(Guid userId) : IRequest<Result<RefreshToken>>;
}
