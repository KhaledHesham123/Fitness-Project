using IdentityService.Data.DBContexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IdentityService.Authorization
{
    public class PermissionAuthorizationHandler
        : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        private readonly IdentityDbContext _context;

        public PermissionAuthorizationHandler(IdentityDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionAuthorizationRequirement requirement)
        {
            // 1) Extract user ID from token
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return;

            Guid uid = Guid.Parse(userId);

            // 2) Load all permissions for this user (roles + direct user permissions)
            var userPermissions = await GetUserPermissionsAsync(uid);

            // 3) Check conditions based on RequireAll flag
            bool isAuthorized = requirement.RequireAll
                ? requirement.Permissions.All(p => userPermissions.Contains(p))
                : requirement.Permissions.Any(p => userPermissions.Contains(p));

            if (isAuthorized)
                context.Succeed(requirement);
        }

        private async Task<HashSet<string>> GetUserPermissionsAsync(Guid userId)
        {
            // Load permissions assigned via roles
            var rolePermissions = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Name)).ToListAsync();

            #region User Permissions
            //Load direct user permissions(if you support them)
            var directPermissions = await _context.UserPermissions
                .Where(up => up.UserId == userId)
                .Select(up => up.Permission.Name)
                .ToListAsync();

            #endregion

            return new HashSet<string>(rolePermissions.Union(directPermissions));
        }
    }
}
