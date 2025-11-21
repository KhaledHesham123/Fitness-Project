using IdentityService.Features.Shared;
using IdentityService.Shared.Entities;
using IdentityService.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Features.Authantication.Queries.GetRolesByUserId
{
    public class GetPermissionsByUserIdHandler : IRequestHandler<GetRolsByUserIdQuery, Result<IEnumerable<GetRolesByUserIdDTO>>>
    {
        private readonly IRepository<Role> _roleRepository;
        public GetPermissionsByUserIdHandler(IRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Result<IEnumerable<GetRolesByUserIdDTO>>> Handle(GetRolsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userRoles = await _roleRepository.GetAll()
                .Where(w => w.UserRoles.Any(u => u.UserId == request.userId))
                .Select(role => new GetRolesByUserIdDTO
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                }).ToListAsync();

            return userRoles is not null && userRoles.Any()
                ? Result<IEnumerable<GetRolesByUserIdDTO>>.SuccessResponse(userRoles, "roles retrieved successfully")
                : Result<IEnumerable<GetRolesByUserIdDTO>>.FailResponse("No roles found for the specified user.");
        }
    }
}
