using IdentityAuthentication.Model.Models;

namespace IdentityAuthentication.Model.Configurations
{
    public class IdentityAuthenticationConfiguration
    {
        public IdentityAuthenticationConfiguration()
        {
        }

        public AuthenticationBaseConfiguration AuthenticationConfiguration { set; get; }

        public AccessTokenConfiguration AccessTokenConfiguration { set; get; }

        public TokenBaseConfiguration RefreshTokenConfiguration { set; get; }

        public RsaVerifySignatureConfiguration? RsaVerifySignatureConfiguration { set; get; }
    }
}
