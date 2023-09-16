using IdentityAuthentication.Model.Models;

namespace IdentityAuthentication.Model.Configurations
{
    public class IdentityAuthenticationConfiguration
    {
        public IdentityAuthenticationConfiguration()
        {
        }

        public AuthenticationConfigurationBase AuthenticationConfiguration { set; get; }

        public AccessTokenConfiguration AccessTokenConfiguration { set; get; }

        public TokenConfigurationBase RefreshTokenConfiguration { set; get; }

        public RsaVerifySignatureConfiguration? RsaVerifySignatureConfiguration { set; get; }
    }
}
