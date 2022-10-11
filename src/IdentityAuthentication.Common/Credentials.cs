using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RSAExtensions;
using System.Security.Cryptography;
using System.Text;

namespace IdentityAuthentication.Common
{
    public class Credentials
    {
        private readonly SecretKeyConfig _secretKeyConfig;
        private readonly AuthenticationConfig _authenticationConfig;

        public Credentials(AuthenticationConfig authenticationConfig, SecretKeyConfig secretKeyConfig)
        {
            _authenticationConfig = authenticationConfig;
            _secretKeyConfig = secretKeyConfig;
        }

        public SigningCredentials GenerateSigningCredentials()
        {
            var signingKey = GeneratePrivateSecurityKey();

            return new SigningCredentials(signingKey, _authenticationConfig.EncryptionAlgorithm);
        }

        private SecurityKey GeneratePrivateSecurityKey()
        {
            if (_authenticationConfig.EncryptionAlgorithm == SecurityAlgorithms.RsaSha256)
            {
                var rsa = RSA.Create();
                rsa.ImportPrivateKey(RSAKeyType.Pkcs8, _secretKeyConfig.RsaPrivateKey);

                return new RsaSecurityKey(rsa);
            }

            var keyByteArray = Encoding.ASCII.GetBytes(_secretKeyConfig.HmacSha256Key);
            return new SymmetricSecurityKey(keyByteArray);
        }


        public static SecurityKey GetSecurityKey(IConfiguration configuration)
        {
            var encryptionAlgorithm = configuration.GetValue<string>("Autnentication:EncryptionAlgorithm");
            var rsaPublicKey = configuration.GetValue<string>("SecretKey:RsaPublicKey");
            var hmacSha256Key = configuration.GetValue<string>("SecretKey:HmacSha256Key");

            if (encryptionAlgorithm.IsNullOrEmpty()) encryptionAlgorithm = SecurityAlgorithms.RsaSha256;
            if (encryptionAlgorithm == SecurityAlgorithms.RsaSha256)
            {
                var rsa = RSA.Create();
                rsa.ImportPublicKey(RSAKeyType.Pkcs8, rsaPublicKey);

                return new RsaSecurityKey(rsa);
            }

            var keyByteArray = Encoding.ASCII.GetBytes(hmacSha256Key);
            return new SymmetricSecurityKey(keyByteArray);
        }
    }
}