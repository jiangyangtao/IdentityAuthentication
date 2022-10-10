using IdentityAuthentication.Extensions;
using Microsoft.IdentityModel.Tokens;
using RSAExtensions;
using System.Security.Cryptography;
using System.Text;

namespace IdentityAuthentication.Core
{
    public static class SecurityKeyHandle
    {
        public static SigningCredentials GetSigningCredentials(string encryptionAlgorithm, string key, bool rsaPrivate = false)
        {
            if (encryptionAlgorithm.IsNullOrEmpty()) encryptionAlgorithm = SecurityAlgorithms.RsaSha256;

            var securityKey = GetSecurityKey(encryptionAlgorithm, key, rsaPrivate);
            if (encryptionAlgorithm == SecurityAlgorithms.RsaSha256) return new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        public static SecurityKey GetSecurityKey(string encryptionAlgorithm, string key, bool rsaPrivate = false)
        {
            if (encryptionAlgorithm.IsNullOrEmpty()) encryptionAlgorithm = SecurityAlgorithms.RsaSha256;

            if (encryptionAlgorithm == SecurityAlgorithms.RsaSha256)
            {
                var rsa = RSA.Create();
                if (rsaPrivate) rsa.ImportPrivateKey(RSAKeyType.Pkcs8, key);
                if (rsaPrivate == false) rsa.ImportPublicKey(RSAKeyType.Pkcs8, key);

                return new RsaSecurityKey(rsa);
            }

            var keyByteArray = Encoding.ASCII.GetBytes(key);
            return new SymmetricSecurityKey(keyByteArray);
        }
    }
}
