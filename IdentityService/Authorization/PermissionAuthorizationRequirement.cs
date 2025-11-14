using Microsoft.AspNetCore.Authorization;

namespace IdentityService.Authorization
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public bool RequireAll { get; }
        public string[] Permissions { get; }

        public PermissionAuthorizationRequirement(bool requireAll, string[] permissions)
        {
            RequireAll = requireAll;
            Permissions = permissions;
        }
    }
}
