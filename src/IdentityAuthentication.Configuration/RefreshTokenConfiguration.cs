using IdentityAuthentication.Model.Models;
using Newtonsoft.Json;

namespace IdentityAuthentication.Configuration
{
    public class RefreshTokenConfiguration : TokenConfigurationBase
    {
        public const string ConfigurationKey = "RefreshToken";

        public override DateTime TokenExpirationTime => DateTime.Now.AddDays(ExpirationTime);
    }
}
