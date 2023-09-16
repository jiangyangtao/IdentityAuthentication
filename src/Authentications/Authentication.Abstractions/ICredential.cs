using IdentityAuthentication.Configuration.Model;

namespace Authentication.Abstractions
{
    public interface ICredential
    {
        public string GrantType { get; }

        public string GrantSource { get; }

        public string Client { get; }

        AuthenticationResult CreateAuthenticationResult(string id, IReadOnlyDictionary<string, string> metadata, string username = "");
    }
}
