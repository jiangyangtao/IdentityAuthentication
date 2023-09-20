using IdentityAuthentication.Configuration.Abstractions;

namespace Authentication.Abstractions
{
    public interface ICredential
    {
        public string GrantType { get; }

        public string GrantSource { get; }

        public string Client { get; }

        IAuthenticationResult CreateAuthenticationResult(string id, IReadOnlyDictionary<string, string> metadata, string username = "");
    }
}
