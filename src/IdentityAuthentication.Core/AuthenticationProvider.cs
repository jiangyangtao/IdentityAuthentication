using IdentityAuthentication.Abstractions;
using IdentityAuthentication.TokenServices.Abstractions;
using Newtonsoft.Json.Linq;

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

        public async Task<bool> AuthorizeAsync() => await _tokenProvider.AuthorizeAsync();

        public async Task<string> RefreshTokenAsync()
        {
            var authenticationResult = await _tokenProvider.GetAuthenticationResultAsync();
            var checkResult = await _authenticationHandle.IdentityCheckAsync(authenticationResult);

            var token = checkResult ? await _tokenProvider.RefreshAsync() : await _tokenProvider.DestroyAsync();
            return token;
        }

        public async Task<IReadOnlyDictionary<string, string>> TokenInfoAsync() => await _tokenProvider.InfoAsync();
    }
}
