using IdentityAuthentication.Abstractions;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Core.Abstractions
{
    internal interface IAuthenticationHandler
    {
        Task<IAuthenticationResult> AuthenticateAsync(JObject credentialObject);

        Task<bool> AuthenticateAsync(IAuthenticationResult authenticationResult);
    }
}
