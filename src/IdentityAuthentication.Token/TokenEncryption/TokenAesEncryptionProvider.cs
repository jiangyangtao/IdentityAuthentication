using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Token.Abstractions;
using System.Security.Cryptography;

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
            var cryptoTransform = Aes.CreateDecryptor();
            return AesHandle(token, cryptoTransform);
        }

        public string Encrypt(string token)
        {
            var cryptoTransform = Aes.CreateEncryptor();
            return AesHandle(token, cryptoTransform);
        }

        private static string AesHandle(string value, ICryptoTransform cryptoTransform)
        {
            var valueBytes = Convert.FromBase64String(value);
            var resultArray = cryptoTransform.TransformFinalBlock(valueBytes, 0, valueBytes.Length);
            return Convert.ToBase64String(resultArray);
        }
    }
}
