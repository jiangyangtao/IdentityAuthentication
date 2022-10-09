using Authentication.Abstractions;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace IdentityAuthentication.Core
{
    internal class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly TokenProvider _tokenProvider;
        private readonly AuthenticationHandle _authenticationHandle;

        public AuthenticationProvider(
            TokenProvider tokenProvider,
            AuthenticationHandle authenticationHandle)
        {
            _tokenProvider = tokenProvider;
            _authenticationHandle = authenticationHandle;
        }

        public async Task<IToken> AuthenticateAsync(JObject credentialObject)
        {
            var authenticationResult = await _authenticationHandle.AuthenticateAsync(credentialObject);
            var token = _tokenProvider.GenerateToken(authenticationResult);

            return await Task.FromResult(token);
        }

        public async Task<IToken> RefreshTokenAsync()
        {
            return await _tokenProvider.RefreshTokenAsync();
        }
    }
}
