
using IdentityAuthentication.Extensions;
using System.Net;

namespace Authentication.Abstractions.Credentials
{
    public class PasswordCredential : ICredential
    {
        public string AuthenticationType => "Password";

        public string Username { set; get; }

        public string Password { set; get; }

        public string AuthenticationSource { set; get; }

        public AuthenticationResult CreateAuthenticationResult(string id, IReadOnlyDictionary<string, string> metadata, string username = "")
        {
            return AuthenticationResult.CreateAuthenticationResult(id, username.NotNullAndEmpty() ? username : Username, AuthenticationType, metadata);
        }
    }
}
