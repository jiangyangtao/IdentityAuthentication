using IdentityAuthentication.Configuration.Model;
using IdentityAuthentication.Model.Enums;
using IdentityAuthentication.Token.Abstractions;
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
        private readonly ITokenEncryptionProviderFactory _tokenEncryptionProviderFactory;

        public EncryptionTokenProvider(ITokenEncryptionProviderFactory tokenEncryptionProviderFactory)
        {
            _tokenEncryptionProviderFactory = tokenEncryptionProviderFactory;
        }

        public TokenType TokenType => TokenType.Encrypt;

        public Task<TokenValidationResult> AuthorizeAsync()
        {
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
