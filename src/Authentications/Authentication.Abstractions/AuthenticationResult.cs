using IdentityAuthentication.Abstractions;

namespace Authentication.Abstractions
{
    internal class AuthenticationResult : IAuthenticationResult
    {
        private AuthenticationResult() { }

        public string UserId { private set; get; }

        public string Username { private set; get; }

        public string GrantSource { private set; get; }

        public string GrantType { private set; get; }

        public string Client { private set; get; }

        public IReadOnlyDictionary<string, string> Metadata { private set; get; }

        public static IAuthenticationResult CreateAuthenticationResult(string userId, string username, string grantSource, string grantType, string client, IReadOnlyDictionary<string, string> metadata)
        {
            return new AuthenticationResult()
            {
                UserId = userId,
                Username = username,
                GrantSource = grantSource,
                GrantType = grantType,
                Client = client,
                Metadata = metadata
            };
        }
    }
}
