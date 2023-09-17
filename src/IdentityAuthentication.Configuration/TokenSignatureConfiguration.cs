using IdentityAuthentication.Configuration.Enums;

namespace IdentityAuthentication.Configuration
{
    public class TokenSignatureConfiguration
    {
        public TokenSignatureAlgorithmType AlgorithmType { set; get; }

        public RsaSignatureConfiguration RsaSignature { set; get; }

        public SymmetricSignatureConfiguration SymmetricSignature { set; get; }

        public bool IsRsaSignature
        {
            get
            {
                if (AlgorithmType > TokenSignatureAlgorithmType.HmacSha512) return true;

                return false;
            }
        }
    }
}
