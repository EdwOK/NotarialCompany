using System;
using NotarialCompany.Models;

namespace NotarialCompany.Security.Authorization
{
    public class ResourceInfo
    {
        public ResourceInfo(Type resourceType, ResourceAction[] resourceActions, User user)
        {
            ResourceType = resourceType;
            ResourceActions = resourceActions;
            User = user;
        }

        public Type ResourceType { get; set; }

        public ResourceAction[] ResourceActions { get; set; }

        public User User { get; set; }
    }
}