﻿using System;
using System.Security.Cryptography;
using System.Text;
using NotarialCompany.DataAccess;
using NotarialCompany.Models;

namespace NotarialCompany.Security.Authentication
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

        public User GenerateCredentials(User user)
        {
            user.Salt = GenerateNewSalt(32);
            user.Password = GenerateHash(user.Password, user.Salt);
            return user;
        }

        public bool IsAuthenticated()
        {
            return CurrentUser != null;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public bool ValidatePassword(string username, string password)
        {
            var user = dbScope.GetUserByUsername(username);
            if (user == null || !CompareHash(password, user.Password, user.Salt))
            {
                return false;
            }

            CurrentUser = user;
            return true;
        }

        private bool CompareHash(string attemptedPassword, string hash, string salt)
        {
            return hash == GenerateHash(attemptedPassword, salt);
        }

        private string GenerateHash(string password, string salt)
        {
            var algorithm = new SHA256Managed();
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