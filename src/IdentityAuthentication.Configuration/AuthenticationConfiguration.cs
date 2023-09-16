using IdentityAuthentication.Model.Configurations;

namespace IdentityAuthentication.Configuration
{
    public class AuthenticationConfiguration : AuthenticationConfigurationBase
    {
        /// <summary>
        /// 启用 token 刷新
        /// </summary>
        public bool EnableTokenRefresh { set; get; }
    }
}
