using IdentityService.Features.Shared;
using MediatR;

namespace IdentityService.Features.Authantication.Queries.GetRolesByUserId
{
    public record GetRolsByUserIdQuery(Guid userId) : IRequest<Result<IEnumerable<GetRolesByUserIdDTO>>>;
}
