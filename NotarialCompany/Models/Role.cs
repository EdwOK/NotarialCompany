using System;
using NotarialCompany.Security;

namespace NotarialCompany.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public RoleDefinition GetRoleDefinition()
        {
            return (RoleDefinition)Enum.Parse(typeof (RoleDefinition), Name);
        }
    }
}
