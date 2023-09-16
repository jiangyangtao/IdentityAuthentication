using IdentityAuthentication.Configuration.Model;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Authentication
{
    public interface IAuthenticationProvider
    {
        Task<AuthenticationResult> AuthenticateAsync(JObject credentialObject);

        Task<bool> AuthenticateAsync(AuthenticationResult authenticationResult);
    }
}