using IdentityAuthentication.Model.Configurations;
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

        /// <summary>
        /// 签名的凭据
        /// </summary>
        /// <returns></returns>
        public SigningCredentials GenerateSigningCredentials()
        {
            var signingKey = GenerateSignatureSecurityKey();

            return new SigningCredentials(signingKey, _authenticationConfig.EncryptionAlgorithm);
        }

        /// <summary>
        /// 验签的凭据
        /// </summary>
        /// <returns></returns>
        public SigningCredentials GenerateValidateSignatureCredentials()
        {
            var signingKey = GenerateValidateSecurityKey();
            return new SigningCredentials(signingKey, _authenticationConfig.EncryptionAlgorithm);
        }

        /// <summary>
        /// 生成签名的 <see cref="SecurityKey"/>
        /// </summary>
        /// <returns></returns>
        public SecurityKey GenerateSignatureSecurityKey() => GenerateSecurityKey(false);

        /// <summary>
        /// 生成验签的 <see cref="SecurityKey"/>
        /// </summary>
        /// <returns></returns>
        public SecurityKey GenerateValidateSecurityKey() => GenerateSecurityKey();

        private SecurityKey GenerateSecurityKey(bool isPublic = true)
        {
            if (_authenticationConfig.EncryptionAlgorithm == SecurityAlgorithms.RsaSha256)
            {
                var rsa = RSA.Create();
                if (isPublic) rsa.ImportPublicKey(RSAKeyType.Pkcs8, _secretKeyConfig.RsaSignaturePublicKey);
                if (isPublic == false) rsa.ImportPrivateKey(RSAKeyType.Pkcs8, _secretKeyConfig.RsaSignaturePrivateKey);

                return new RsaSecurityKey(rsa);
            }

            var keyByteArray = Encoding.ASCII.GetBytes(_secretKeyConfig.HmacSha256Key);
            return new SymmetricSecurityKey(keyByteArray);
        }
    }
}
