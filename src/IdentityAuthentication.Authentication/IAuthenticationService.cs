using IdentityAuthentication.Abstractions;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Authentication
{
    public interface IAuthenticationService
    {
        Task<IAuthenticationResult> AuthenticateAsync(JObject credentialObject);

        Task<bool> AuthenticateAsync(IAuthenticationResult authenticationResult);
    }
}