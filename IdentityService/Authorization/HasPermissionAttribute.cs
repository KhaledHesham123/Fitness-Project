using Microsoft.AspNetCore.Authorization;

namespace IdentityService.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public bool RequireAll { get; }
        public string[] Permissions { get; }
        public HasPermissionAttribute(params string[] permissions)
        : this(false, permissions) { }
        public HasPermissionAttribute(bool requireAll, string[] permissions)
        {
            RequireAll = requireAll;
            Permissions = permissions;

            // Generate the policy name dynamically
            Policy = BuildPolicyName(requireAll, permissions);
        }

        private static string BuildPolicyName(bool requireAll, string[] permissions)
        {
            var mode = requireAll ? "ALL" : "ANY";
            var joined = string.Join(",", permissions);
            return $"Permission:{mode}:{joined}";

            // Permission:ALL:User.Create,User.Delete

        }
    }
}
