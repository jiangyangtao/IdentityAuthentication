using IdentityAuthentication.Model.Configurations;
using RSAExtensions;
using System.Security.Cryptography;

namespace IdentityAuthentication.Model
{
    public class RsaAlgorithm
    {
        private readonly SecretKeyConfiguration _secretKeyConfig;

        public RsaAlgorithm(SecretKeyConfiguration secretKeyConfig)
        {
            _secretKeyConfig = secretKeyConfig;
        }

        private RSA _PublicRSA { set; get; }

        public RSA PublicRSA
        {
            get
            {
                if (_PublicRSA == null)
                {
                    if (string.IsNullOrEmpty(_secretKeyConfig.RsaEncryptPublicKey)) throw new NullReferenceException("RsaEncryptPublicKey is null or empty");

                    _PublicRSA = RSA.Create();
                    _PublicRSA.ImportPublicKey(RSAKeyType.Pkcs8, _secretKeyConfig.RsaEncryptPublicKey);
                }

                return _PublicRSA;
            }
        }

        private RSA _PrivateRSA { set; get; }

        public RSA PrivateRSA
        {
            get
            {
                if (_PrivateRSA == null)
                {
                    if (string.IsNullOrEmpty(_secretKeyConfig.RsaDecryptPrivateKey)) throw new NullReferenceException("RsaDecryptPrivateKey is null or empty");

                    _PrivateRSA = RSA.Create();
                    _PrivateRSA.ImportPrivateKey(RSAKeyType.Pkcs8, _secretKeyConfig.RsaDecryptPrivateKey);
                }

                return _PrivateRSA;
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public string Encrypt(string plaintext)
        {
            var result = PublicRSA.EncryptBigData(plaintext, RSAEncryptionPadding.Pkcs1);

            return result;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="ciphertext"></param>
        /// <returns></returns>
        public string Decrypt(string ciphertext)
        {
            _PrivateRSA = RSA.Create();
            _PrivateRSA.ImportPrivateKey(RSAKeyType.Pkcs8, _secretKeyConfig.RsaDecryptPrivateKey);
            var result = _PrivateRSA.DecryptBigData(ciphertext, RSAEncryptionPadding.Pkcs1);

            return result;
        }
    }
}
