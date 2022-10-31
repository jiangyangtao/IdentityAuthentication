using IdentityAuthenticaion.Model.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RSAExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthenticaion.Model
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
                if (isPublic) rsa.ImportPublicKey(RSAKeyType.Pkcs8, _secretKeyConfig.RsaPrivateKey);
                if (isPublic == false) rsa.ImportPrivateKey(RSAKeyType.Pkcs8, _secretKeyConfig.RsaPrivateKey);

                return new RsaSecurityKey(rsa);
            }

            return GenerateSymmetricSecurityKey(_secretKeyConfig.HmacSha256Key);
        }

        public static SecurityKey GetEncryptSecurityKey(IConfiguration configuration)
        {
            var encryptionAlgorithm = configuration.GetValue<string>("EncryptionAlgorithm");
            var rsaPublicKey = configuration.GetValue<string>("RsaPublicKey");
            var hmacSha256Key = configuration.GetValue<string>("HmacSha256Key");

            if (encryptionAlgorithm.IsNullOrEmpty()) encryptionAlgorithm = SecurityAlgorithms.RsaSha256;
            if (encryptionAlgorithm == SecurityAlgorithms.RsaSha256)
            {
                var rsa = RSA.Create();
                rsa.ImportPublicKey(RSAKeyType.Pkcs8, rsaPublicKey);

                return new RsaSecurityKey(rsa);
            }

            return GenerateSymmetricSecurityKey(hmacSha256Key);
        }

        private static SymmetricSecurityKey GenerateSymmetricSecurityKey(string key)
        {
            var keyByteArray = Encoding.ASCII.GetBytes(key);
            return new SymmetricSecurityKey(keyByteArray);
        }
    }
}
