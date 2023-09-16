using IdentityAuthentication.Configuration.Model;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Authentication
{
    internal class AuthenticationProvider : IAuthenticationProvider
    {
        public Task<AuthenticationResult> AuthenticateAsync(JObject credentialObject)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AuthenticateAsync(AuthenticationResult authenticationResult)
        {
            throw new NotImplementedException();
        }
    }
}
