using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Model.Configurations;

namespace IdentityAuthentication.Configuration
{
    public class AuthenticationConfiguration : AuthenticationConfigurationBase
    {
        /// <summary>
        /// 启用 token 刷新
        /// </summary>
        public bool EnableTokenRefresh { set; get; }

        public TokenSignatureType TokenSignatureType { set; get; }

        public TokenEncryptionType TokenEncryptionType { set; get; }

        public const string ConfigurationKey = "Authentication";
    }
}
