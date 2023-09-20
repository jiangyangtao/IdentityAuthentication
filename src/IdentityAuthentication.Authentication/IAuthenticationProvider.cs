using IdentityAuthentication.Configuration.Abstractions;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Authentication
{
    public interface IAuthenticationProvider
    {
        Task<IAuthenticationResult> AuthenticateAsync(JObject credentialObject);

        Task<bool> AuthenticateAsync(IAuthenticationResult authenticationResult);
    }
}