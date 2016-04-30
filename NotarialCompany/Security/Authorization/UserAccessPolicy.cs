using System;
using System.Linq;
using NotarialCompany.Models;

namespace NotarialCompany.Security.Authorization
{
    public class UserAccessPolicy : AccessPolicyBase
    {
        internal static readonly Type[] StandardAccessTypes =
        {
            typeof(Client),
            typeof(Deal)
        };

        internal static readonly Type[] ReadOnlyAccessTypes =
        {
            typeof(Employee),
            typeof(EmployeesPosition),
            typeof(Service)
        };

        public override bool CheckAccess(ResourceInfo resourceInfo)
        {
            if (!IsInRole(resourceInfo, RoleDefinition.User))
            {
                return false;
            }
            return (StandardAccessTypes.Contains(resourceInfo.ResourceType) &&
                    resourceInfo.ResourceActions.All(ra => StandartResourceActions.Contains(ra))) ||
                   (ReadOnlyAccessTypes.Contains(resourceInfo.ResourceType) &&
                    resourceInfo.ResourceActions.All(a => a == ResourceAction.Read));
        }
    }
}
