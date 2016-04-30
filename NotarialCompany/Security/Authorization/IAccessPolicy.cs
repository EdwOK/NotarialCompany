namespace NotarialCompany.Security.Authorization
{
    public interface IAccessPolicy
    {
        bool CheckAccess(ResourceInfo context);
    }
}
