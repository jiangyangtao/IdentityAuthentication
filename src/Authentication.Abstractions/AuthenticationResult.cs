

namespace Authentication.Abstractions
{
    public sealed class AuthenticationResult
    {
        private AuthenticationResult(string userId, string username, string authenticationSource, IReadOnlyDictionary<string, string> metadata)
        {
            UserId = userId;
            Username = username;
            AuthenticationSource = authenticationSource;

            var meta = BuildMetaData(authenticationSource);
            foreach (var item in metadata)
            {
                if (item.Key.Equals(nameof(AuthenticationSource), StringComparison.OrdinalIgnoreCase)) continue;

                meta.Add(item.Key, item.Value);
            }
            Metadata = meta;
        }

        public string UserId { get; }

        public string Username { get; }

        public string AuthenticationSource { get; }

        private Dictionary<string, string> BuildMetaData(string authenticationSource)
        {
            return new Dictionary<string, string> { { nameof(AuthenticationSource), authenticationSource } };
        }

        public IReadOnlyDictionary<string, string> Metadata { get; }

        public static AuthenticationResult CreateAuthenticationResult(string userId, string username, string authenticationSource, IReadOnlyDictionary<string, string> metadata)
        {
            return new(userId, username, authenticationSource, metadata);
        }

    }
}
