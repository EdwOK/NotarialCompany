using NotarialCompany.Models;

namespace NotarialCompany.Security.Authentication
{
    public interface IAuthenticationService
    {
        User CurrentUser { get; }

        User GenerateCredentials(User user);

        bool ValidatePassword(string username, string password);

        void Logout();

        bool IsAuthenticated();
    }
}