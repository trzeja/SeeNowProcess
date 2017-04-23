using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace SeeNowProcess.Models
{
    public class CryptoServices
    {
        public static Boolean CheckPassword(string attemptedPassword, byte[] hash, byte[] salt)
        {
            Boolean result = true;
            byte[] attemptedHash = Encrypt(attemptedPassword, salt);
            if (attemptedHash.GetLength(0) != hash.GetLength(0))
                return false;
            for (int i = 0; i < hash.GetLength(0); ++i)
            {
                if (attemptedHash[i] != hash[i])
                    result = false;
            }
            return result;
        }

        public static byte[] GenerateSalt()
        {
            byte[] result = new byte[8];
            using (RNGCryptoServiceProvider Csp = new RNGCryptoServiceProvider())
            {
                Csp.GetBytes(result);
            }
            return result;
        }

        public static byte[] Encrypt(string password, byte[] salt)
        {
            return new Rfc2898DeriveBytes(password, salt, 10000).GetBytes(16);
        }
        
    }
}