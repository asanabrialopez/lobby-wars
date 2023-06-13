﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LobbyWars.SharedKernel.Services
{
    public static class PasswordHasher
    {

        public static string ComputeHash(string password, string salt, string pepper)
        {
            using var sha256 = SHA256.Create();
            var passwordSaltPepper = $"{password}{salt}{pepper}";
            var byteValue = Encoding.UTF8.GetBytes(passwordSaltPepper);
            var byteHash = sha256.ComputeHash(byteValue);
            var hash = Convert.ToBase64String(byteHash);
            return hash;
        }

        public static string GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            var byteSalt = new byte[16];
            rng.GetBytes(byteSalt);
            var salt = Convert.ToBase64String(byteSalt);
            return salt;
        }
    }
}
