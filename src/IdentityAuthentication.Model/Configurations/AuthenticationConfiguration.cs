using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.Model.Configurations
{
    public class AuthenticationConfiguration
    {
        public string EncryptionAlgorithm { set; get; } = SecurityAlgorithms.RsaSha256;

        public TokenType TokenType { set; get; } = TokenType.JWT;

        public bool EnableGrpcConnection { get; set; }

        public bool EnableJwtEncrypt { get; set; } = false;
    }

    public enum TokenType
    {
        /// <summary>
        /// Json Web Token
        /// </summary>
        JWT = 1,

        /// <summary>
        /// Reference Token
        /// </summary>
        Reference = 2
    }
}
