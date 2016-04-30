using NotarialCompany.Models;

namespace NotarialCompany.Security.Authorization
{
    public class AdminAccessPolicy : AccessPolicyBase
    {
        public override bool CheckAccess(ResourceInfo resourceInfo)
        {
            return IsInRole(resourceInfo, RoleDefinition.Admin);
        }
    }
}
