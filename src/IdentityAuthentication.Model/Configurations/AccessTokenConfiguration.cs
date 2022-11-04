
namespace IdentityAuthentication.Model.Configurations
{
    public class AccessTokenConfiguration
    {
        public long ExpirationTime { set; get; }

        public long RefreshTime { set; get; } = 0;

        public string Issuer { set; get; }

        public string Audience { set; get; }
    }
}
