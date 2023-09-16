
using System.Security.Claims;

namespace IdentityAuthentication.Model.Handles
{
    public static class IdentityAuthenticationDefaults
    {
        public const string AuthenticationScheme = "IdentityAuthentication";

        public const string JwtAuthenticationScheme = "IdentityServerAuthenticationJwt";

        public const string AccessToken = "access_token";

        public const string RefreToken = "refresh-token";

        public const string Authorization = "Authorization";

        /// <summary>
        /// 过期时间
        /// </summary>
        public const string Expiration = ClaimTypes.Expiration;

        /// <summary>
        /// 签发时间
        /// </summary>
        public const string IssueTime = "IssueTime";
    }
}
