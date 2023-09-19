using IdentityAuthentication.Model.Models;

namespace IdentityAuthentication.Configuration
{
    public class RefreshTokenConfiguration : TokenConfigurationBase
    {
        public const string ConfigurationKey = "RefreshToken";

        public DateTime TokenExpirationTime => DateTime.Now.AddDays(ExpirationTime);
    }
}
