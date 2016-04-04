using NotarialCompany.Models;

namespace NotarialCompany.Security
{
    public interface IAuthenticationService
    {
        User CurrentUser { get; }

        void GenerateCredentials(User user);

        bool ValidatePassword(string username, string password);

        void Logout();

        bool IsAuthenticated();
    }
}