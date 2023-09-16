
namespace IdentityAuthentication.Model.Configurations
{
    public class IdentityAuthenticationConfiguration
    {
        public IdentityAuthenticationConfiguration()
        {
        }

        public AuthenticationConfiguration AuthenticationConfiguration { set; get; }

        public AccessTokenConfiguration AccessTokenConfiguration { set; get; }

        public TokenConfigurationBase RefreshTokenConfiguration { set; get; }

        public RsaVerifySignatureConfiguration? RsaVerifySignatureConfiguration { set; get; }
    }
}
