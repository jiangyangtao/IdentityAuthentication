using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Configuration.Model;
using IdentityAuthentication.Model.Enums;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.Token
{
    internal class JwtTokenProvider : ITokenProvider
    {
        private readonly IAuthenticationConfigurationProvider _configurationProvider;

        public JwtTokenProvider(IAuthenticationConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public TokenType TokenType => throw new NotImplementedException();

        public Task<TokenValidationResult> AuthorizeAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task<string> DestroyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ITokenResult> GenerateAsync(AuthenticationResult authenticationResult)
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