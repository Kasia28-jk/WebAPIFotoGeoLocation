using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;

namespace FotoGeoLocationWebApplication.Login
{
    public class HashProvider : IHashProvider
    {
        public string GetEncryptedPassword(string password)
        {
            byte[] salt = Encoding.UTF8.GetBytes("zaqwsxcdejiminek");
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: password,
                            salt: salt,
                            prf: KeyDerivationPrf.HMACSHA256,
                            iterationCount: 100000,
                            numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
