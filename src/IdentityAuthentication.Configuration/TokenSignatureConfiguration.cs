using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Model.Enums;

namespace IdentityAuthentication.Configuration
{
    public class TokenSignatureConfiguration
    {
        public TokenSignatureAlgorithmType AlgorithmType { set; get; }

        public RsaSignatureConfiguration RsaSignature { set; get; }

        public SymmetricSignatureConfiguration SymmetricSignature { set; get; }

        public RsaAlgorithmType? RsaAlgorithm
        {
            get
            {
                if (AlgorithmType == TokenSignatureAlgorithmType.RsaSha256) return RsaAlgorithmType.RsaSha256;
                if (AlgorithmType == TokenSignatureAlgorithmType.RsaSha384) return RsaAlgorithmType.RsaSha384;
                if (AlgorithmType == TokenSignatureAlgorithmType.RsaSha512) return RsaAlgorithmType.RsaSha512;

                return null;
            }
        }

        public bool IsRsaSignature
        {
            get
            {
                if (AlgorithmType > TokenSignatureAlgorithmType.HmacSha512) return true;

                return false;
            }
        }

        public const string ConfigurationKey = "TokenSignature";
    }
}
