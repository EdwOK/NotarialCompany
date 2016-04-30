﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NotarialCompany.Security.Authentication;

namespace NotarialCompany.Security.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IList<IAccessPolicy> accessPolicies;

        public AuthorizationService(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
            this.accessPolicies = GetAccessPolicies();
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
            Type accessPolicyType = typeof(IAccessPolicy);
            var accessPoliciesTypes =
                GetType().Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(accessPolicyType) && !t.IsAbstract);

            return accessPoliciesTypes.Select(
                accessPoliciesType => (IAccessPolicy) Activator.CreateInstance(accessPoliciesType)).ToList();
        }
    }
}
