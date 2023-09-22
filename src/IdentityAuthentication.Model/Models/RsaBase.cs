using IdentityAuthentication.Model.Enums;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RSAExtensions;

namespace IdentityAuthentication.Model.Models
{
    public abstract class RsaBase
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public RsaAlgorithmType AlgorithmType { set; get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RSAKeyType RSAKeyType { set; get; }

        [JsonIgnore]
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
