using IdentityAuthentication.Model.Configurations;

namespace IdentityAuthentication.Configuration
{
    public class IdentityAuthenticationConfiguration : AuthenticationConfiguration
    {
        /// <summary>
        /// 启用 token 刷新
        /// </summary>
        public bool EnableTokenRefresh { set; get; }
    }
}
