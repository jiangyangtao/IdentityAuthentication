using Authentication.Abstractions;
using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
using IdentityAuthentication.TokenServices.Abstractions;
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
        private readonly AuthenticationHandle _authenticationHandle;
        private readonly ITokenProvider _tokenProvider;

        public AuthenticationProvider(
            AuthenticationHandle authenticationHandle,
           ITokenProviderFactory tokenProviderFactory)
        {
            _tokenProvider = tokenProviderFactory.CreateTokenService();
            _authenticationHandle = authenticationHandle;
        }

        public async Task<IToken> AuthenticateAsync(JObject credentialObject)
        {
            var authenticationResult = await _authenticationHandle.AuthenticateAsync(credentialObject);
            var token = await _tokenProvider.GenerateAsync(authenticationResult);

            return token;
        }

        public async Task<string> RefreshTokenAsync()
        {
            var authenticationResult = await _tokenProvider.GetAuthenticationResultAsync();
            var checkResult = await _authenticationHandle.IdentityCheckAsync(authenticationResult);

            var token = checkResult ? await _tokenProvider.RefreshAsync() : await _tokenProvider.DestroyAsync();
            return token;
        }
    }
}
