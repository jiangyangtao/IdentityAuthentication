using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Authentication;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.Core
{
    internal class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenProvider _tokenProvider;

        public AuthenticationProvider(
           ITokenProviderFactory tokenProviderFactory,
           IAuthenticationService authenticationService)
        {
            _tokenProvider = tokenProviderFactory.CreateTokenService();
            _authenticationService = authenticationService;
        }

        public async Task<IToken> AuthenticateAsync(JObject credentialObject)
        {
            var authenticationResult = await _authenticationService.AuthenticateAsync(credentialObject);
            var token = await _tokenProvider.GenerateAsync(authenticationResult);

            return token;
        }

        public async Task<TokenValidationResult> ValidateTokenAsync(string token) => await _tokenProvider.AuthorizeAsync();

        public async Task<string> RefreshTokenAsync()
        {
            var authenticationResult = await _tokenProvider.GetAuthenticationResultAsync() ?? throw new Exception("Authentication failed;");
            var checkResult = await _authenticationService.AuthenticateAsync(authenticationResult);
            var token = checkResult ? await _tokenProvider.RefreshAsync() : await _tokenProvider.DestroyAsync();

            return token;
        }

        public async Task<IReadOnlyDictionary<string, string>> GetTokenInfoAsync()
        {
            var dic = await _tokenProvider.GetTokenInfoAsync();
            if (dic == null) return null;

            return dic;
        }
    }
}
