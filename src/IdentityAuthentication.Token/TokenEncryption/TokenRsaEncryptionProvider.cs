using IdentityAuthentication.Configuration;
using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Token.Abstractions;
using RSAExtensions;
using System.Security.Cryptography;

namespace IdentityAuthentication.Token.TokenEncryption
{
    internal class TokenRsaEncryptionProvider : ITokenEncryptionProvider
    {
        private readonly RSA Rsa;
        private readonly RsaEncryptionConfiguration RsaEncryptionConfiguration;

        public TokenRsaEncryptionProvider(IAuthenticationConfigurationProvider authenticationConfigurationProvider)
        {
            if (authenticationConfigurationProvider.RsaEncryption == null) throw new NullReferenceException(nameof(authenticationConfigurationProvider.RsaEncryption));

            RsaEncryptionConfiguration = authenticationConfigurationProvider.RsaEncryption;
            Rsa = RSA.Create();
            Rsa.ImportPrivateKey(RsaEncryptionConfiguration.RSAKeyType, RsaEncryptionConfiguration.PrivateKey);
            Rsa.ImportPrivateKey(RsaEncryptionConfiguration.RSAKeyType, RsaEncryptionConfiguration.PublicKey);
        }

        public TokenEncryptionType EncryptionType => TokenEncryptionType.Rsa;

        public string Decrypt(string token) => Rsa.DecryptBigData(token, RsaEncryptionConfiguration.RSAEncryptionPadding);

        public string Encrypt(string token) => Rsa.EncryptBigData(token, RsaEncryptionConfiguration.RSAEncryptionPadding);
    }
}
