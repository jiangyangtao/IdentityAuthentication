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

        public EncryptionTokenProvider(
            ITokenEncryptionProviderFactory tokenEncryptionProviderFactory,
            IHttpTokenProvider httpTokenProvider)
        {
            _tokenEncryptionProvider = tokenEncryptionProviderFactory.CreateTokenEncryptionProvider();
            _httpTokenProvider = httpTokenProvider;
        }

        public TokenType TokenType => TokenType.Encrypt;

        public async Task<TokenValidationResult> AuthorizeAsync()
        {
            if (_httpTokenProvider.AccessTokenIsEmpty) return TokenValidation.FailedTokenValidationResult;

            var tokenJson = _tokenEncryptionProvider.Decrypt(_httpTokenProvider.AccessToken);
            if (tokenJson.IsNullOrEmpty()) return TokenValidation.FailedTokenValidationResult;

            var r = TokenInfo.TryParse(tokenJson, out TokenInfo? tokenInfo);
            if (r == false) return TokenValidation.FailedTokenValidationResult;
            if (tokenInfo == null) return TokenValidation.FailedTokenValidationResult;

            if (DateTime.Now > tokenInfo.ExpirationTime) return TokenValidation.FailedTokenValidationResult;

            var claims = tokenInfo.BuildClaims();
            var identity = new ClaimsIdentity(claims, tokenInfo.GrantType);
            var result = new TokenValidationResult { IsValid = true, ClaimsIdentity = identity, };
            return await Task.FromResult(result);
        }

        public Task<string> DestroyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IToken> GenerateAsync(AuthenticationResult authenticationResult)
        {
            throw new NotImplementedException();
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
