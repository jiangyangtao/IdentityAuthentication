using IdentityAuthentication.Configuration;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Authentication
{
    internal class AuthenticationService : IAuthenticationService
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
