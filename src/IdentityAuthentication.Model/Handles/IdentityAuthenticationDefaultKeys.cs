using System.Security.Claims;

namespace IdentityAuthentication.Model.Handles
{
    public static class IdentityAuthenticationDefaultKeys
    {
        public const string AuthenticationScheme = "IdentityAuthentication";

        public const string JwtAuthenticationScheme = "IdentityServerAuthenticationJwt";

        public const string AccessToken = "access_token";

        public const string RefreToken = "refresh-token";

        public const string Authorization = "Authorization";

        public const string Expiration = ClaimTypes.Expiration;

        public const string NotBefore = nameof(NotBefore);

        public const string IssueTime = nameof(IssueTime);
    }
}
