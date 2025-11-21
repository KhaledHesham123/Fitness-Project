using IdentityService.Features.Shared;
using IdentityService.Shared.Entities;
using IdentityService.Shared.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityService.Features.Authantication.Queries.GetPermissionsByUserId
{
    public class GetPermissionsByUserIdHandler : IRequestHandler<GetPermissionsByUserIdQuery, Result<IEnumerable<GetPermissionsByUserIdDTO>>>
    {
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IMemoryCache _cache;

        public GetPermissionsByUserIdHandler(IRepository<Permission> permissionRepository, IMemoryCache cache)
        {
            _permissionRepository = permissionRepository;
            _cache = cache;

        }

        public async Task<Result<IEnumerable<GetPermissionsByUserIdDTO>>> Handle(GetPermissionsByUserIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"permissions_user_{request.userId}";

            //if (_cache.TryGetValue(cacheKey, out List<string> cachedPermissions))
            //    return cachedPermissions;

            var userpermission = await _permissionRepository.GetAll()
                .Where(p =>
                    p.UserPermissions.Any(u => u.UserId == request.userId) ||
                    p.RolePermissions.Any(rp => rp.Role.UserRoles.Any(ur => ur.UserId == request.userId)
                )).Select(perm => new GetPermissionsByUserIdDTO
                {
                    PermissionId = perm.Id,
                    PermissionName = perm.Name,
                }).Distinct().ToListAsync();

            // Cache for 30 minutes
            //_cache.Set(cacheKey, permissions, TimeSpan.FromMinutes(30));

            return userpermission is not null && userpermission.Any()
                ? Result<IEnumerable<GetPermissionsByUserIdDTO>>.SuccessResponse(userpermission, "permissions retrieved successfully")
                : Result<IEnumerable<GetPermissionsByUserIdDTO>>.FailResponse("No permissions found for the specified user.");
        }
    }
}
