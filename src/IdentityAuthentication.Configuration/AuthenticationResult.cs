using System.Security.Claims;
using IdentityAuthentication.Extensions;

namespace IdentityAuthentication.Configuration
{
    public class AuthenticationResult
    {
        private AuthenticationResult(string userId, string username, string grantSource, string grantType, string client, IReadOnlyDictionary<string, string> metadata)
        {
            UserId = userId;
            Username = username;
            GrantSource = grantSource;
            GrantType = grantType;
            Client = client;
            Metadata = metadata;
        }

        public static string UserIdPropertyName => nameof(UserId);

        public static string UsernamePropertyName => nameof(Username);

        public static string GrantSourcePropertyName => nameof(GrantSource);

        public static string GrantTypePropertyName => nameof(GrantType);

        public static string ClientPropertyName => nameof(Client);

        public string UserId { get; }

        public string Username { get; }

        public string GrantSource { get; }

        public string GrantType { get; }

        public string Client { get; }

        public IReadOnlyDictionary<string, string> Metadata { get; }

        public Claim[] GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(UserIdPropertyName, UserId),
                new Claim(UsernamePropertyName,Username),
                new Claim(GrantSourcePropertyName,GrantSource),
                new Claim(GrantTypePropertyName,GrantType),
                new Claim(ClientPropertyName,Client)
            };
            if (Metadata.IsNullOrEmpty()) return claims.ToArray();

            foreach (var item in Metadata)
            {
                claims.Add(new Claim(item.Key, item.Value));
            }

            return claims.ToArray();
        }

        public IReadOnlyDictionary<string, string> ToReadOnlyDictionary()
        {
            var dic = new Dictionary<string, string>
            {
                {UserIdPropertyName, UserId},
                {UsernamePropertyName,Username},
                {GrantSourcePropertyName,GrantType},
                {GrantTypePropertyName,GrantSource},
                {ClientPropertyName,Client},
            };
            if (Metadata.IsNullOrEmpty()) return dic;

            foreach (var item in Metadata)
            {
                dic.Add(item.Key, item.Value);
            }

            return dic;
        }

        public static AuthenticationResult CreateAuthenticationResult(string userId, string username, string grantSource, string grantType, string client, IReadOnlyDictionary<string, string> metadata)
        {
            return new(userId, username, grantSource, grantType, client, metadata);
        }

    }
}
