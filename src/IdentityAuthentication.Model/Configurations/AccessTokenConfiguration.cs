
namespace IdentityAuthentication.Model.Configurations
{
    public class AccessTokenConfiguration : TokenConfigurationBase
    {
        public long RefreshTime { set; get; } = 0;
    }
}
