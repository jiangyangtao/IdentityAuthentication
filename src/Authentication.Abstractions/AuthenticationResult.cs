

using IdentityAuthentication.Extensions;

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

            var meta = BuildMetaData();
            if (metadata.NotNullAndEmpty())
            {
                foreach (var item in metadata)
                {
                    if (item.Key.Equals(nameof(AuthenticationSource), StringComparison.OrdinalIgnoreCase)) continue;
                    if (item.Key.Equals(nameof(AuthenticationType), StringComparison.OrdinalIgnoreCase)) continue;

                    meta.Add(item.Key, item.Value);
                }
            }

            Metadata = meta;
        }

        public string UserId { get; }

        public string Username { get; }

        public string AuthenticationSource { get; }

        public string AuthenticationType { get; }

        private Dictionary<string, string> BuildMetaData()
        {
            return new Dictionary<string, string> {
                { nameof(AuthenticationSource), AuthenticationSource },
                { nameof(AuthenticationType), AuthenticationType }
            };
        }

        public IReadOnlyDictionary<string, string> Metadata { get; }

        public static AuthenticationResult CreateAuthenticationResult(string userId, string username, string authenticationSource, string authenticationType, IReadOnlyDictionary<string, string> metadata)
        {
            return new(userId, username, authenticationSource, authenticationType, metadata);
        }

    }
}
