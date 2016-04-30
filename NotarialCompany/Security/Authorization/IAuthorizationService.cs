using System;

namespace NotarialCompany.Security.Authorization
{
    public interface IAuthorizationService
    {
        bool CheckAccess(Type resource, params ResourceAction[] actions);
    }
}