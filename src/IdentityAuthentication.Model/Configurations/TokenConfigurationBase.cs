
namespace IdentityAuthentication.Model.Configurations
{
    public abstract class TokenConfigurationBase
    {
        public long ExpirationTime { set; get; }

        public string Issuer { set; get; }

        public string Audience { set; get; }
    }
}
