
using IdentityAuthentication.Model.Models;
using Newtonsoft.Json;

namespace IdentityAuthentication.Model.Configurations
{
    public class AccessTokenConfiguration : TokenConfigurationBase
    {
        public long RefreshTime { set; get; } = 0;

        public const string ConfigurationKey = "AccessToken";

        public override DateTime TokenExpirationTime => DateTime.Now.AddSeconds(ExpirationTime);
    }
}
