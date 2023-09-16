using IdentityAuthentication.Model.Enums;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.Model.Configurations
{
    public class AuthenticationConfiguration
    {
        public string EncryptionAlgorithm { set; get; } = SecurityAlgorithms.RsaSha256;

        public TokenType TokenType { set; get; } = TokenType.JWT;

        public bool EnableGrpcConnection { get; set; }
    }
}
