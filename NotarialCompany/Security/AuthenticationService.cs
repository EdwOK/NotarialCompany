using System;
using System.Security.Cryptography;
using System.Text;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;


namespace NotarialCompany.Security
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly DbScope dbScope;
        private readonly Encoding encoding = Encoding.Default;

        public AuthenticationService(DbScope dbScope)
        {
            this.dbScope = dbScope;
        }

        public User CurrentUser { get; private set; }

        public void GenerateCredentials(User user)
        {
            user.Salt = GenerateNewSalt();
            user.Password = GenerateHash(user.Password, user.Salt);

            dbScope.UpdateUser(user);
        }

        public bool ValidatePassword(string username, string password)
        {
            CurrentUser = dbScope.GetUserByUsernameAndPassword(username, password);
            return CurrentUser != null;
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public bool IsAuthenticated()
        {
            return CurrentUser != null;
        }

        private string GenerateHash(string password, string salt)
        {
            var algorithm = new SHA1Managed();
            return encoding.GetString(algorithm.ComputeHash(encoding.GetBytes(password + salt)));
        }

        private string GenerateNewSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var buff = new byte[64];
                rng.GetBytes(buff);
                return encoding.GetString(buff);
            }
        }
    }
}
