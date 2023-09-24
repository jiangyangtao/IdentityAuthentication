using IdentityAuthentication.Configuration;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Token.Abstractions;
using RSAExtensions;
using System.Security.Cryptography;
using System.Text;

namespace IdentityAuthentication.Token.TokenEncryption
{
    internal class TokenRsaEncryptionProvider : ITokenEncryptionProvider
    {
        private readonly RSA PublicRsa;
        private readonly RSA PrivateRsa;
        private readonly RsaEncryptionConfiguration RsaEncryptionConfiguration;

        public TokenRsaEncryptionProvider(IAuthenticationConfigurationProvider authenticationConfigurationProvider)
        {
            if (authenticationConfigurationProvider.RsaEncryption == null) throw new ArgumentException(nameof(authenticationConfigurationProvider.RsaEncryption));

            RsaEncryptionConfiguration = authenticationConfigurationProvider.RsaEncryption;
            PublicRsa = RSA.Create();
            PrivateRsa = RSA.Create();
            PrivateRsa.ImportPrivateKey(RsaEncryptionConfiguration.RSAKeyType, RsaEncryptionConfiguration.PrivateKey);
            PublicRsa.ImportPublicKey(RsaEncryptionConfiguration.RSAKeyType, RsaEncryptionConfiguration.PublicKey);
        }

        public TokenEncryptionType EncryptionType => TokenEncryptionType.Rsa;

        public string Decrypt(string token) => PrivateRsa.DecryptBigData(token, RsaEncryptionConfiguration.RSAEncryptionPadding);

        public string Encrypt(string token) => PublicRsa.EncryptBigData(token, RsaEncryptionConfiguration.RSAEncryptionPadding);
    }
}
