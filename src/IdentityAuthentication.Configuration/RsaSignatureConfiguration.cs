using IdentityAuthentication.Model;
using IdentityAuthentication.Model.Configurations;
using IdentityAuthentication.Model.Models;

namespace IdentityAuthentication.Configuration
{
    public class RsaSignatureConfiguration : RsaBase
    {
        public string PrivateKey { set; get; }

        public string PublicKey { set; get; }

        public Credentials GetPublicCredentials()
        {
            var publicSignature = new RsaSignature()
            {
                RSAKeyType = RSAKeyType,
                AlgorithmType = AlgorithmType,
                SignatureKey = PublicKey,
                IsPublic = true,
            };
            return new Credentials(publicSignature);
        }

        public Credentials GetPrivateCredentials()
        {
            var publicSignature = new RsaSignature()
            {
                RSAKeyType = RSAKeyType,
                AlgorithmType = AlgorithmType,
                SignatureKey = PrivateKey,
                IsPublic = false,
            };
            return new Credentials(publicSignature);
        }

        public RsaVerifySignatureConfiguration BuildRsaVerifySignatureConfiguration()
        {
            return new()
            {
                RSAKeyType = RSAKeyType,
                PublicKey = PublicKey,
                AlgorithmType = AlgorithmType,
            };
        }

        public const string ConfigurationKey = "TokenRsaSignature";
    }
}
