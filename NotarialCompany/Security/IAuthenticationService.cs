using System.Threading.Tasks;
using NotarialCompany.Models;

namespace NotarialCompany.Security
{
    public interface IAuthenticationService
    {
        User CurrentUser { get; }

        User GenerateCredentials(User user);

        Task<bool> ValidatePassword(string username, string password);

        void Logout();

        bool IsAuthenticated();
    }
}