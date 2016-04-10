using System;
using System.Security.Cryptography;
using System.Text;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;


namespace NotarialCompany.Security
{
    public class AuthenticationService : IAuthenticationService
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
            user.Salt = GenerateNewSalt(64);
            user.Password = GenerateHash(user.Password, user.Salt);
            dbScope.UpdateUser(user);
        }

        public bool ValidatePassword(string username, string password)
        {
            CurrentUser = dbScope.GetUserByUsernameAndPassword(username, password);
            return CurrentUser != null && !CompareHash(password, CurrentUser.Password, CurrentUser.Salt);
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public bool IsAuthenticated()
        {
            return CurrentUser != null;
        }

        private bool CompareHash(string attemptedPassword, string hash, string salt)
        {
            return hash == GenerateHash(attemptedPassword, salt);
        }

        private string GenerateHash(string password, string salt)
        {
            var algorithm = new SHA512Managed();
            var unhashedBytes = encoding.GetBytes(string.Concat(salt, password));
            var hashedBytes = algorithm.ComputeHash(unhashedBytes);
            return Convert.ToBase64String(hashedBytes);
        }

        private string GenerateNewSalt(int length)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var buff = new byte[length];
                rng.GetBytes(buff);
                return Convert.ToBase64String(buff);
            }
        }
    }
}
