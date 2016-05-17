using System;
using System.Linq;
using NotarialCompany.Models;

namespace NotarialCompany.Security.Authorization
{
    public abstract class AccessPolicyBase : IAccessPolicy
    {
        public static ResourceAction[] StandartResourceActions { get; } = {
            ResourceAction.Read, ResourceAction.Create,
            ResourceAction.Update, ResourceAction.Delete
        };

        public abstract bool CheckAccess(ResourceInfo resourceInfo);

        public bool CheckStandartAction(ResourceInfo resourceInfo, Type resourceType, ResourceAction resourceAction)
        {
            return resourceInfo.ResourceType == resourceType
                   && resourceInfo.ResourceActions.Any(ra => ra == resourceAction);
        }

        public bool IsInRole(ResourceInfo resourceInfo, RoleDefinition roleDefinition)
        {
            return GetRoleDefinition(resourceInfo.User.Role.Name) == roleDefinition;
        }

        private RoleDefinition GetRoleDefinition(string name)
        {
            return (RoleDefinition) Enum.Parse(typeof(RoleDefinition), name);
        }
    }
}