using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Model.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.Configuration
{
    public class SymmetricSignatureConfiguration : SymmetricSignature
    {
        public SymmetricSignatureAlgorithmType SignatureAlgorithm { set; get; }

        public override string Algorithm
        {
            get
            {
                if (SignatureAlgorithm == SymmetricSignatureAlgorithmType.HmacSha256) return SecurityAlgorithms.HmacSha256;
                if (SignatureAlgorithm == SymmetricSignatureAlgorithmType.HmacSha384) return SecurityAlgorithms.HmacSha384;

                return SecurityAlgorithms.HmacSha512;
            }
        }

        public const string ConfigurationKey = "TokenSymmetricSignature";
    }
}
