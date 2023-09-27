using IdentityAuthentication.Model.Models;

namespace IdentityAuthentication.Configuration
{
    public class RefreshTokenConfiguration : TokenBaseConfiguration
    {
        public const string ConfigurationKey = "RefreshToken";

        public DateTime TokenExpirationTime => DateTime.Now.AddDays(ExpirationTime);
    }
}
