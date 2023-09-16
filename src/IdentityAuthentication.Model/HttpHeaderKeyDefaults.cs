using System.Security.Claims;

namespace IdentityAuthentication.Model
{
    public class HttpHeaderKeyDefaults
    {
        public const string AccessToken = "access_token";
        public const string RefreToken = "refresh-token";
        public const string Authorization = "Authorization";
    }

    public class ClaimKeyDefaults
    {
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
