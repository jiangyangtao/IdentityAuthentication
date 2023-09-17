using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Model.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.Configuration
{
    public class SymmetricSignatureConfiguration : SymmetricSignature
    {
        public TokenSignatureAlgorithmType SignatureAlgorithm { set; get; }

        public override string Algorithm
        {
            get
            {
                if (SignatureAlgorithm == TokenSignatureAlgorithmType.HmacSha256) return SecurityAlgorithms.HmacSha256;
                if (SignatureAlgorithm == TokenSignatureAlgorithmType.HmacSha384) return SecurityAlgorithms.HmacSha384;

                return SecurityAlgorithms.HmacSha512;
            }
        }
    }
}
