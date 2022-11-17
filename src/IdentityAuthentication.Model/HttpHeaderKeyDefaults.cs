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
        public const string Expiration = ClaimTypes.Expiration;
        public const string IssueTime = "IssueTime";
    }
}
