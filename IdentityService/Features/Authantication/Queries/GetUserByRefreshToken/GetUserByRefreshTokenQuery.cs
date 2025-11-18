using IdentityService.Features.Shared;
using IdentityService.Shared.Entities;
using MediatR;

namespace IdentityService.Features.Authantication.Queries.GetUserByRefreshToken
{
    public record GetUserByRefreshTokenQuery(string refreshToken) : IRequest<Result<User>>;
}
