using IdentityAuthentication.Model.Enums;

namespace IdentityAuthentication.Model.Configurations
{
    public abstract class AuthenticationConfigurationBase
    {
        public TokenType TokenType { set; get; } = TokenType.JWT;

        public bool EnableGrpcConnection { get; set; }
    }
}
