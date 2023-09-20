using IdentityAuthentication.Configuration.Abstractions;

namespace Authentication.Abstractions
{
    internal class AuthenticationResult : IAuthenticationResult
    {
        public string UserId { set; get; }

        public string Username { set; get; }

        public string GrantSource { set; get; }

        public string GrantType { set; get; }

        public string Client { set; get; }

        public IReadOnlyDictionary<string, string> Metadata { set; get; }
    }
}
