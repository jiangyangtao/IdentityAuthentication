using IdentityAuthentication.Model.Configurations;
using Microsoft.Extensions.Options;
using RSAExtensions;
using System.Security.Cryptography;
using System.Text;

namespace IdentityAuthentication.TokenServices.Providers
{
    internal class RsaAlgorithmProvider
    {
        private readonly SecretKeyConfiguration _secretKeyConfig;

        public RsaAlgorithmProvider(IOptions<SecretKeyConfiguration> secretKeyOption)
        {
            _secretKeyConfig = secretKeyOption.Value;
        }

        private RSA _PublicProvider { set; get; }

        public RSA PublicProvider
        {
            get
            {

                if (_PublicProvider == null)
                {

                    _PublicProvider = RSA.Create();
                    _PublicProvider.ImportPrivateKey(RSAKeyType.Pkcs8, _secretKeyConfig.RsaPublicKey);
                }

                return _PublicProvider;
            }
        }

        private RSA _PrivateProvider { set; get; }

        public RSA PrivateProvider
        {
            get
            {
                if (_PrivateProvider == null)
                {

                    _PrivateProvider = RSA.Create();
                    _PrivateProvider.ImportPrivateKey(RSAKeyType.Pkcs8, _secretKeyConfig.RsaPrivateKey);
                }

                return _PrivateProvider;
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public string Encrypt(string plaintext)
        {
            var data = Encoding.UTF8.GetBytes(plaintext);
            var r = _PublicProvider.Encrypt(data, RSAEncryptionPadding.Pkcs1);

            return Convert.ToBase64String(r);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public string Decrypt(string ciphertext)
        {
            var data = Convert.FromBase64String(ciphertext);
            var r = _PrivateProvider.Decrypt(data, RSAEncryptionPadding.Pkcs1);

            return Encoding.UTF8.GetString(r);
        }
    }
}
