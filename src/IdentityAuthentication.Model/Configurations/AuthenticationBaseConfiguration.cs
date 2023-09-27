using IdentityAuthentication.Model.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IdentityAuthentication.Model.Configurations
{
    public class AuthenticationBaseConfiguration
    {
        [JsonConverter(typeof(StringEnumConverter))]
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
