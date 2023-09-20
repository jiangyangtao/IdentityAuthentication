using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Configuration.Model;
using IdentityAuthentication.Model;
using IdentityAuthentication.Model.Enums;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityAuthentication.Token
{
    internal class EncryptionTokenProvider : ITokenProvider
    {
        private readonly ITokenEncryptionProvider _tokenEncryptionProvider;
        private readonly IHttpTokenProvider _httpTokenProvider;
        private readonly IAuthenticationConfigurationProvider _configurationProvider;

        public EncryptionTokenProvider(
            ITokenEncryptionProviderFactory tokenEncryptionProviderFactory,
            IHttpTokenProvider httpTokenProvider,
            IAuthenticationConfigurationProvider configurationProvider)
        {
            _tokenEncryptionProvider = tokenEncryptionProviderFactory.CreateTokenEncryptionProvider();
            _httpTokenProvider = httpTokenProvider;
            _configurationProvider = configurationProvider;
        }

        public TokenType TokenType => TokenType.Encrypt;

        public async Task<TokenValidationResult> AuthorizeAsync()
        {
            var (r, tokenInfo) = ValidateAccessToken();
            if (r == false) return TokenValidation.FailedTokenValidationResult;

            var claims = tokenInfo.BuildClaims();
            var identity = new ClaimsIdentity(claims, tokenInfo.GrantType);
            var result = new TokenValidationResult { IsValid = true, ClaimsIdentity = identity, };
            return await Task.FromResult(result);
        }

        private (bool result, TokenInfo? tokenInfo) ValidateAccessToken()
        {
            if (_httpTokenProvider.AccessTokenIsEmpty) return (false, null);

            var tokenJson = _tokenEncryptionProvider.Decrypt(_httpTokenProvider.AccessToken);
            if (tokenJson.IsNullOrEmpty()) return (false, null);

            var r = TokenInfo.TryParse(tokenJson, out TokenInfo? tokenInfo);
            if (r == false) return (false, null);
            if (tokenInfo == null) return (false, null);

            if (DateTime.Now > tokenInfo.ExpirationTime) return (false, null);

            return (true, tokenInfo);
        }

        public Task<string> DestroyAsync()
        {
            var (r, tokenInfo) = ValidateAccessToken();
            if (r == false) return Task.FromResult(string.Empty);

            tokenInfo.ExpirationTime = tokenInfo.IssueTime.AddSeconds(1);
            var json = tokenInfo.ToString();
            var accessToken = _tokenEncryptionProvider.Encrypt(json);
            return Task.FromResult(accessToken);
        }

        public Task<IToken> GenerateAsync(AuthenticationResult authenticationResult)
        {
            var tokenInfo = TokenInfo.CreateToken(authenticationResult);
            tokenInfo.ExpirationTime = _configurationProvider.AccessToken.TokenExpirationTime;
            tokenInfo.IssueTime = DateTime.Now;
            tokenInfo.NotBefore = DateTime.Now;

            var json = tokenInfo.ToString();
            var accessToken = _tokenEncryptionProvider.Encrypt(json);
            var token = TokenResult.CreateToken(accessToken, _configurationProvider.AccessToken.ExpirationTime, tokenInfo.BuildDictionary());
        }

        public Task<AuthenticationResult> GetAuthenticationResultAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyDictionary<string, string>?> GetTokenInfoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> RefreshAsync()
        {
            throw new NotImplementedException();
        }
    }
}
