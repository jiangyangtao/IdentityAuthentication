
namespace IdentityAuthentication.Model.Configurations
{
    public class AccessTokenConfiguration : RefreshTokenConfiguration
    {
        public long RefreshTime { set; get; } = 0;
    }
}
