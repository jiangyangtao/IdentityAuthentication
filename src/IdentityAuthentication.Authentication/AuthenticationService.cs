using IdentityAuthentication.Abstractions;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Authentication
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationHandle _authenticationHandle;

        public AuthenticationService(AuthenticationHandle authenticationHandle)
        {
            _authenticationHandle = authenticationHandle;
        }

        public Task<IAuthenticationResult> AuthenticateAsync(JObject credentialObject) => _authenticationHandle.AuthenticateAsync(credentialObject);


        public Task<bool> AuthenticateAsync(IAuthenticationResult authenticationResult) => _authenticationHandle.AuthenticateAsync(authenticationResult);
    }
}
