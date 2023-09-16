using IdentityAuthentication.Model.Enums;
using Microsoft.IdentityModel.Tokens;
using RSAExtensions;

namespace IdentityAuthentication.Model.Models
{
    public abstract class RsaBase
    {
        public RsaAlgorithmType AlgorithmType { set; get; }

        public RSAKeyType RSAKeyType { set; get; }

        public string Algorithm
        {
            get
            {
                if (AlgorithmType == RsaAlgorithmType.RsaSha256) return SecurityAlgorithms.RsaSha256;
                if (AlgorithmType == RsaAlgorithmType.RsaSha384) return SecurityAlgorithms.RsaSha384;

                return SecurityAlgorithms.RsaSha512;
            }
        }
    }
}
