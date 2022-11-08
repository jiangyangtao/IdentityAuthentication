using IdentityAuthentication.Model.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RSAExtensions;
using System.Security.Cryptography;
using System.Text;

namespace IdentityAuthentication.Model
{
    public class Credentials
    {
        private readonly SecretKeyConfiguration _secretKeyConfig;
        private readonly AuthenticationConfiguration _authenticationConfig;

        public Credentials(AuthenticationConfiguration authenticationConfig, SecretKeyConfiguration secretKeyConfig)
        {
            _authenticationConfig = authenticationConfig;
            _secretKeyConfig = secretKeyConfig;
        }

        public SigningCredentials GenerateSigningCredentials()
        {
            var signingKey = GenerateDecryptionSecurityKey();

            return new SigningCredentials(signingKey, _authenticationConfig.EncryptionAlgorithm);
        }

        /// <summary>
        /// 生成加密的 <see cref="SecurityKey"/>
        /// </summary>
        /// <returns></returns>
        public SecurityKey GenerateDecryptionSecurityKey() => GenerateSecurityKey(false);

        /// <summary>
        /// 生成解密的 <see cref="SecurityKey"/>
        /// </summary>
        /// <returns></returns>
        public SecurityKey GenerateEncryptSecurityKey() => GenerateSecurityKey();

        private SecurityKey GenerateSecurityKey(bool isPublic = true)
        {
            if (_authenticationConfig.EncryptionAlgorithm == SecurityAlgorithms.RsaSha256)
            {
                var rsa = RSA.Create();
                if (isPublic) rsa.ImportPublicKey(RSAKeyType.Pkcs8, _secretKeyConfig.RsaPublicKey);
                if (isPublic == false) rsa.ImportPrivateKey(RSAKeyType.Pkcs8, _secretKeyConfig.RsaPrivateKey);

                return new RsaSecurityKey(rsa);
            }

            var keyByteArray = Encoding.ASCII.GetBytes(_secretKeyConfig.HmacSha256Key);
            return new SymmetricSecurityKey(keyByteArray);
        }
    }
}
