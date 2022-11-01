using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthenticaion.Model.Configurations
{
    public class AuthenticationConfiguration
    {
        public string EncryptionAlgorithm { set; get; } = SecurityAlgorithms.RsaSha256;

        public TokenType TokenType { set; get; } = TokenType.JWT;

        /// <summary>
        /// Enable grpc connection
        /// </summary>
        public bool EnableGrpcConnection { get; set; } = true;

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
