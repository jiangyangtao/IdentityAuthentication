using IdentityAuthentication.Cache.Abstractions;
using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Model;
using IdentityAuthentication.Model.Enums;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityAuthentication.Token
{
    internal class ReferenceTokenProvider : ITokenProvider
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IHttpTokenProvider _httpTokenProvider;
        private readonly IAuthenticationConfigurationProvider _authenticationConfigurationProvider;

        public ReferenceTokenProvider(
            ICacheProviderFactory cacheProviderFactory,
            IHttpTokenProvider httpTokenProvider,
            IAuthenticationConfigurationProvider authenticationConfigurationProvider)
        {
            _cacheProvider = cacheProviderFactory.CreateCacheProvider();
            _httpTokenProvider = httpTokenProvider;
            _authenticationConfigurationProvider = authenticationConfigurationProvider;
        }

        public TokenType TokenType => TokenType.Reference;

        public async Task<TokenValidationResult> AuthorizeAsync()
        {
            try
            {
                if (_httpTokenProvider.AccessToken.IsNullOrEmpty()) return TokenValidation.FailedTokenValidationResult;

                var tokenInfo = await _cacheProvider.GetAsync<TokenInfo>(_httpTokenProvider.AccessToken);
                if (tokenInfo == null) return TokenValidation.FailedTokenValidationResult;
                if (DateTime.Now > tokenInfo.ExpirationTime) return TokenValidation.FailedTokenValidationResult;

                var claims = tokenInfo.BuildClaims();
                var identity = new ClaimsIdentity(claims, tokenInfo.GrantType);
                return new TokenValidationResult { IsValid = true, ClaimsIdentity = identity, };
            }
            catch
            {
                return TokenValidation.FailedTokenValidationResult;
            }
        }

        public async Task<string> DestroyAsync()
        {
            if (_httpTokenProvider.AccessTokenIsEmpty) return string.Empty;

            await _cacheProvider.RemoveAsync(_httpTokenProvider.AccessToken);
            return _httpTokenProvider.AccessToken;
        }

        public async Task<IToken> GenerateAsync(IAuthenticationResult authenticationResult)
        {
            var token = TokenInfo.CreateToken(authenticationResult);
            token.ExpirationTime = _authenticationConfigurationProvider.AccessToken.TokenExpirationTime;

            var accessToken = Guid.NewGuid().ToString();
            await _cacheProvider.SetAsync(accessToken, token);
            return TokenResult.CreateToken(accessToken, _authenticationConfigurationProvider.AccessToken.ExpirationTime, token.BuildDictionary());
        }

        public async Task<IAuthenticationResult> GetAuthenticationResultAsync()
        {
            if (_httpTokenProvider.AccessToken.IsNullOrEmpty()) return null;

            var token = await _cacheProvider.GetAsync<TokenInfo>(_httpTokenProvider.AccessToken);
            if (token == null) return null;

            return token.AuthenticationResult;
        }

        public async Task<IReadOnlyDictionary<string, string>?> GetTokenInfoAsync()
        {
            if (_httpTokenProvider.AccessToken.IsNullOrEmpty()) return null;

            var token = await _cacheProvider.GetAsync<TokenInfo>(_httpTokenProvider.AccessToken);
            if (token == null) return null;

            return token.BuildDictionary();
        }

        public async Task<string> RefreshAsync()
        {
            if (_httpTokenProvider.AccessToken.IsNullOrEmpty()) return string.Empty;
            if (_authenticationConfigurationProvider.Authentication.EnableTokenRefresh == false) return string.Empty;

            var token = await _cacheProvider.GetAsync<TokenInfo>(_httpTokenProvider.AccessToken);
            token.ExpirationTime = _authenticationConfigurationProvider.AccessToken.TokenExpirationTime;

            await _cacheProvider.SetAsync(_httpTokenProvider.AccessToken, token);
            return _httpTokenProvider.AccessToken;
        }
    }
}
