using IdentityAuthentication.Configuration.Abstractions;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Authentication
{
    internal class AuthenticationProvider : IAuthenticationProvider
    {
        public Task<IAuthenticationResult> AuthenticateAsync(JObject credentialObject)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AuthenticateAsync(IAuthenticationResult authenticationResult)
        {
            throw new NotImplementedException();
        }
    }
}
