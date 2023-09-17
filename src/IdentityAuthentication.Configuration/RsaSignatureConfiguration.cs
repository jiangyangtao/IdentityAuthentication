using IdentityAuthentication.Model.Models;

namespace IdentityAuthentication.Configuration
{
    public class RsaSignatureConfiguration : RsaBase
    {
        public string PrivateKey { set; get; }

        public string PublicKey { set; get; }

        public RsaSignature ToPublicRsa()
        {
            return new()
            {
                RSAKeyType = RSAKeyType,
                AlgorithmType = AlgorithmType,
                SignatureKey = PublicKey,
            };
        }

        public RsaSignature ToPrivateRsa()
        {
            return new()
            {
                RSAKeyType = RSAKeyType,
                AlgorithmType = AlgorithmType,
                SignatureKey = PrivateKey,
            };
        }

        public const string ConfigurationKey = "TokenRsaSignature";
    }
}
