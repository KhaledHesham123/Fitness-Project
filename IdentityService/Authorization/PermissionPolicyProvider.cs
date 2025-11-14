using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace IdentityService.Authorization
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        #region Sample 1
        //public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        //public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        //{
        //    FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        //}

        //public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        //    => FallbackPolicyProvider.GetFallbackPolicyAsync();

        //public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        //    => FallbackPolicyProvider.GetDefaultPolicyAsync();

        //public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        //{
        //    var policy = new AuthorizationPolicyBuilder();
        //    policy.AddRequirements(new PermissionRequirement(policyName));
        //    return Task.FromResult(policy.Build());
        //} 
        #endregion

        public const string PolicyPrefix = "Permission:";
        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;
        private readonly IMemoryCache _cache;

        public PermissionPolicyProvider(IConfiguration config,
            IMemoryCache cache, IOptions<AuthorizationOptions> options)
        {
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            _cache = cache;
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
            => _fallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
            => _fallbackPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase))
                return _fallbackPolicyProvider.GetPolicyAsync(policyName);

            // cache key same as policyName
            if (_cache.TryGetValue(policyName, out AuthorizationPolicy cached))
                return Task.FromResult(cached);

            // parse: Permission:MODE:perm1,perm2
            var withoutPrefix = policyName.Substring(PolicyPrefix.Length);
            // expected: MODE:perm1,perm2
            var idx = withoutPrefix.IndexOf(':');
            if (idx <= 0)
                return Task.FromResult<AuthorizationPolicy?>(null);

            var mode = withoutPrefix.Substring(0, idx).ToUpperInvariant(); // "ANY" or "ALL"
            var perms = withoutPrefix.Substring(idx + 1)
                .Split(',', StringSplitOptions.RemoveEmptyEntries |
                            StringSplitOptions.TrimEntries);

            var requireAll = mode == "ALL";

            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionAuthorizationRequirement(requireAll, perms))
                .Build();

            // cache for a while (configurable) to avoid recreation overhead
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));

            _cache.Set(policyName, policy, cacheEntryOptions);

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

    }

}
