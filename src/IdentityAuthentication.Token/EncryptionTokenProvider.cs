using IdentityAuthentication.Configuration.Model;
using IdentityAuthentication.Model;
using IdentityAuthentication.Model.Enums;
using IdentityAuthentication.Token.Abstractions;
using IdentityAuthentication.Token.TokenEncryption;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<TokenValidationResult> AuthorizeAsync()
        {
            if (_httpTokenProvider.AccessTokenIsEmpty) return TokenValidation.FailedTokenValidationResult;

            var tokenJson = _tokenEncryptionProvider.Decrypt(_httpTokenProvider.AccessToken);
            if (tokenJson.IsNullOrEmpty()) return TokenValidation.FailedTokenValidationResult;

            var r = AuthenticationResult.TryParse(tokenJson)

            throw new NotImplementedException();
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
