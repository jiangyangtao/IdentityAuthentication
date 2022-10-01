
namespace Authentication.Abstractions
{
    public interface ICredential
    {
        public string AuthenticationType { get; }

        public string AuthenticationSource { get; }

        AuthenticationResult CreateAuthenticationResult(string id, IReadOnlyDictionary<string, string> metadata, string username = "");
    }
}
