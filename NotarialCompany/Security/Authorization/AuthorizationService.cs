using System;
using System.Collections.Generic;
using System.Linq;
using NotarialCompany.Security.Authentication;

namespace NotarialCompany.Security.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IList<IAccessPolicy> accessPolicies;

        private readonly IAuthenticationService authenticationService;

        public AuthorizationService(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
            accessPolicies = GetAccessPolicies();
        }

        public bool CheckAccess(Type resource, params ResourceAction[] actions)
        {
            if (!authenticationService.IsAuthenticated())
            {
                return false;
            }

            var resourceInfo = new ResourceInfo(resource, actions, authenticationService.CurrentUser);
            return accessPolicies.Any(p => p.CheckAccess(resourceInfo));
        }

        private IList<IAccessPolicy> GetAccessPolicies()
        {
            var accessPolicyType = typeof(IAccessPolicy);
            var accessPoliciesTypes =
                GetType()
                    .Assembly.GetTypes()
                    .Where(t => t.GetInterfaces().Contains(accessPolicyType) && !t.IsAbstract);

            return
                accessPoliciesTypes.Select(
                    accessPoliciesType => (IAccessPolicy) Activator.CreateInstance(accessPoliciesType)).ToList();
        }
    }
}