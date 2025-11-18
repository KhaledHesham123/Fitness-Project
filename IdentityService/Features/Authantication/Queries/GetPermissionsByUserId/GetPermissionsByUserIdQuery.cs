using IdentityService.Features.Shared;
using MediatR;

namespace IdentityService.Features.Authantication.Queries.GetPermissionsByUserId
{
    public record GetPermissionsByUserIdQuery(Guid userId) : IRequest<Result<IEnumerable<GetPermissionsByUserIdDTO>>>;
}
