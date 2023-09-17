using IdentityAuthentication.Configuration.Enums;

namespace IdentityAuthentication.Configuration
{
    public class TokenSignatureConfiguration
    {
        public TokenSignatureAlgorithmType AlgorithmType { set; get; }

        public RsaSignatureConfiguration RsaSignature { set; get; }

        public SymmetricSignatureConfiguration SymmetricSignature { set; get; }
    }
}
