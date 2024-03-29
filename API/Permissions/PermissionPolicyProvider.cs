using Common.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace API.Permissions
{
	public class PermissionPolicyProvider : IAuthorizationPolicyProvider
	{
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; set; }
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);            
        }
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if(policyName.StartsWith(AppClaim.Permission, StringComparison.CurrentCultureIgnoreCase))
            {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(policyName));
                return Task.FromResult(policy.Build());
            } // this is to dynamically add our permissions as policies in the app
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }

		public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
		    => FallbackPolicyProvider.GetDefaultPolicyAsync();

		public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
		    => Task.FromResult<AuthorizationPolicy>(null);
	}
}
