using System.Security.Claims;

namespace IdentityAuthentication.Model
{
    public class HttpHeaderKeyDefaults
    {
        public const string AccessToken = "access_token";
        public const string RefreToken = "refresh-token";
        public const string Authorization = "Authorization";
        public const string Expiration = ClaimTypes.Expiration;
    }
}
