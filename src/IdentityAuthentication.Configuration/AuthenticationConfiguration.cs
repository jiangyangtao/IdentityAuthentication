using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Model.Configurations;
using Newtonsoft.Json;

namespace IdentityAuthentication.Configuration
{
    public class AuthenticationConfiguration : AuthenticationBaseConfiguration
    {
        [JsonIgnore]
        public TokenSignatureType TokenSignatureType { set; get; }

        [JsonIgnore]
        public TokenEncryptionType TokenEncryptionType { set; get; }

        public const string ConfigurationKey = "Authentication";

        /// <summary>
        /// 是否为 Jwt token 并且 Rsa 签名
        /// </summary>
        [JsonIgnore]
        public bool IsJwtAndRsaSignature
        {
            get
            {
                if (TokenType == Model.Enums.TokenType.JWT && TokenSignatureType == TokenSignatureType.Rsa)
                    return true;

                return false;
            }
        }
    }
}
