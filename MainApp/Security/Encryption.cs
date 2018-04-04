using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace MainApp.Security
{
    public class Encryption
    {
        public static string GenerateSaltedHash(byte[] password, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[password.Length + salt.Length];

            for (int i = 0; i < password.Length; i++)
            {
                plainTextWithSaltBytes[i] = password[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[password.Length + i] = salt[i];
            }

            return Convert.ToBase64String(algorithm.ComputeHash(plainTextWithSaltBytes));
        }

        public static byte[] GenerateSalt(int length)
        {
            byte[] salt = new byte[length];

            using (RNGCryptoServiceProvider RNGCryptoService = new RNGCryptoServiceProvider())
            {
                RNGCryptoService.GetNonZeroBytes(salt);
            }

            return salt;
        }
    }
}