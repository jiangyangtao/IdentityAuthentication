using IdentityAuthentication.Model.Enums;

namespace IdentityAuthentication.Model.Configurations
{
    public abstract class AuthenticationConfigurationBase
    {
        public TokenType TokenType { set; get; } = TokenType.JWT;

        /// <summary>
        /// 启用 token 刷新
        /// </summary>
        public bool EnableTokenRefresh { set; get; }

        /// <summary>
        /// 启用 Grpc 连接
        /// </summary>
        public bool EnableGrpcConnection { get; set; }
    }
}
