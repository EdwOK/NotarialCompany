using System;
using System.Linq;
using NotarialCompany.Models;

namespace NotarialCompany.Security.Authorization
{
    public abstract class AccessPolicyBase : IAccessPolicy
    {
        public static ResourceAction[] StandartResourceActions { get; } =
        {
            ResourceAction.Read,
            ResourceAction.Create,
            ResourceAction.Update,
            ResourceAction.Delete
        };

        public abstract bool CheckAccess(ResourceInfo resourceInfo);

        public bool IsInRole(ResourceInfo resourceInfo, RoleDefinition roleDefinition)
        {
            return resourceInfo.User.Role.GetRoleDefinition() == roleDefinition;
        }

        public bool CheckStandartAction(ResourceInfo resourceInfo, Type resourceType, ResourceAction resourceAction)
        {
            return resourceInfo.ResourceType == resourceType &&
                   resourceInfo.ResourceActions.Any(ra => ra == resourceAction);
        }
    }
}
