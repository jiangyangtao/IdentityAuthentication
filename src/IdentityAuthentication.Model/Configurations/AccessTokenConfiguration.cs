
using IdentityAuthentication.Model.Models;

namespace IdentityAuthentication.Model.Configurations
{
    public class AccessTokenConfiguration : TokenBaseConfiguration
    {
        public long RefreshTime { set; get; } = 0;

        public const string ConfigurationKey = "AccessToken";

        public DateTime TokenExpirationTime => DateTime.Now.AddSeconds(ExpirationTime);
    }
}
