using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Model.Models;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.Configuration
{
    public class SymmetricSignatureConfiguration : SymmetricSignature
    {
        public JwtSignatureAlgorithm SignatureAlgorithm { set; get; }

        public override string Algorithm
        {
            get
            {
                if (SignatureAlgorithm == JwtSignatureAlgorithm.HmacSha256) return SecurityAlgorithms.HmacSha256;
                if (SignatureAlgorithm == JwtSignatureAlgorithm.HmacSha384) return SecurityAlgorithms.HmacSha384;

                return SecurityAlgorithms.HmacSha512;
            }
        }
    }
}
