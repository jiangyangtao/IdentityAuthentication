using Authentication.Abstractions;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> AuthenticateAsync(JObject credentialObject);

        Task<bool> AuthenticateAsync(AuthenticationResult authenticationResult);
    }
}