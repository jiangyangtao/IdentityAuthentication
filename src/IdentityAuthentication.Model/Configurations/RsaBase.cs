using IdentityAuthentication.Model.Enums;
using Microsoft.IdentityModel.Tokens;
using RSAExtensions;

namespace IdentityAuthentication.Model.Configurations
{
    public abstract class RsaBase
    {
        public RsaSignatureAlgorithm RsaSignatureAlgorithm { set; get; }

        public RSAKeyType RSAKeyType { set; get; }

        public string RsaAlgorithm
        {
            get
            {
                if (RsaSignatureAlgorithm == RsaSignatureAlgorithm.RsaSha256) return SecurityAlgorithms.RsaSha256;
                if (RsaSignatureAlgorithm == RsaSignatureAlgorithm.RsaSha384) return SecurityAlgorithms.RsaSha384;

                return SecurityAlgorithms.RsaSha512;
            }
        }
    }
}
