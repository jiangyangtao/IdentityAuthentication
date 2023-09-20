using IdentityAuthentication.Abstractions;

namespace Authentication.Abstractions
{
    public interface ICredential
    {
        public string GrantType { get; }

        public string GrantSource { get; }

        public string Client { get; }

        static string GrantSourcePropertyName => nameof(GrantSource);

        static string GrantTypePropertyName => nameof(GrantType);

        static string ClientPropertyName => nameof(Client);


        IAuthenticationResult CreateAuthenticationResult(string id, IReadOnlyDictionary<string, string> metadata, string username = "");
    }
}
