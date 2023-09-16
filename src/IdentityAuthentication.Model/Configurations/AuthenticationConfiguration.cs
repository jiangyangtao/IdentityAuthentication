using IdentityAuthentication.Model.Enums;

namespace IdentityAuthentication.Model.Configurations
{
    public class AuthenticationConfiguration
    {
        public TokenType TokenType { set; get; } = TokenType.JWT;

        public bool EnableGrpcConnection { get; set; }

        public RsaSignatureAlgorithm? RsaSignatureAlgorithm { set; get; }
    }
}
