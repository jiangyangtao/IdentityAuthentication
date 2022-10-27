

using IdentityAuthentication.Extensions;
using System.Security.Claims;

namespace Authentication.Abstractions
{
    public sealed class AuthenticationResult
    {
        private AuthenticationResult(string userId, string username, string authenticationSource, string authenticationType, IReadOnlyDictionary<string, string> metadata)
        {
            UserId = userId;
            Username = username;
            AuthenticationSource = authenticationSource;
            AuthenticationType = authenticationType;
            Metadata = metadata;
        }

        public string UserId { get; }

        public string Username { get; }

        public string AuthenticationSource { get; }

        public string AuthenticationType { get; }

        public IReadOnlyDictionary<string, string> Metadata { get; }

        public IReadOnlyList<Claim> GetMetadataClaims()
        {
            var claims = new List<Claim>();
            if (Metadata.IsNullOrEmpty()) return claims;

            foreach (var item in Metadata)
            {
                claims.Add(new Claim(item.Key, item.Value));
            }

            return claims;
        }

        public static AuthenticationResult CreateAuthenticationResult(string userId, string username, string authenticationSource, string authenticationType, IReadOnlyDictionary<string, string> metadata)
        {
            return new(userId, username, authenticationSource, authenticationType, metadata);
        }

    }
}
