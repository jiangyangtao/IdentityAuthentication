
namespace IdentityAuthentication.Model.Configurations
{
    public class AccessTokenConfiguration : TokenBase
    {
        public long RefreshTime { set; get; } = 0;
    }
}
