using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Model.Configurations;

namespace IdentityAuthentication.Configuration
{
    public class AuthenticationConfiguration : AuthenticationConfigurationBase
    {
        public TokenSignatureType TokenSignatureType { set; get; }

        public TokenEncryptionType TokenEncryptionType { set; get; }

        public const string ConfigurationKey = "Authentication";

        /// <summary>
        /// 是否为 Jwt token 并且 Rsa 签名
        /// </summary>
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
