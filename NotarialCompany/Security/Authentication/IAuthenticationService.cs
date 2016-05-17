using NotarialCompany.Models;

namespace NotarialCompany.Security.Authentication
{
    public interface IAuthenticationService
    {
        User CurrentUser { get; }

        User GenerateCredentials(User user);

        bool IsAuthenticated();

        void Logout();

        bool ValidatePassword(string username, string password);
    }
}