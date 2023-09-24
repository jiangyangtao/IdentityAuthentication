using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Token.Abstractions;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IdentityAuthentication.Token.TokenEncryption
{
    internal class TokenAesEncryptionProvider : ITokenEncryptionProvider
    {
        private readonly Aes Aes;

        public TokenAesEncryptionProvider(IAuthenticationConfigurationProvider authenticationConfigurationProvider)
        {
            if (authenticationConfigurationProvider.AesEncryption == null) throw new ArgumentException(nameof(authenticationConfigurationProvider.AesEncryption));

            Aes = Aes.Create();
            Aes.Key = authenticationConfigurationProvider.AesEncryption.AesKey;
            Aes.IV = authenticationConfigurationProvider.AesEncryption.AesIV;
            Aes.Mode = CipherMode.CBC;
            Aes.Padding = PaddingMode.PKCS7;
        }

        public TokenEncryptionType EncryptionType => TokenEncryptionType.Aes;

        public string Decrypt(string token)
        {
            var valueBytes = Convert.FromBase64String(token);
            var cryptoTransform = Aes.CreateDecryptor();
            var resultArray = cryptoTransform.TransformFinalBlock(valueBytes, 0, valueBytes.Length);
            return Encoding.UTF8.GetString(resultArray);
        }

        public string Encrypt(string token)
        {
            var bytes = Encoding.UTF8.GetBytes(token);
            var cryptoTransform = Aes.CreateEncryptor();
            var resultArray = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(resultArray);
        }
    }
}
