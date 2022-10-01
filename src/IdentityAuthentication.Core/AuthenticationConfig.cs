
namespace IdentityAuthentication.Core
{
    internal class AuthenticationConfig
    {
        public long TokenExpirationTime { set; get; }

        public long TokenRefreshTime { set; get; }

        public string Secret { set; get; }

        public string Issuer { set; get; }

        public string Audience { set; get; }
    }
}
