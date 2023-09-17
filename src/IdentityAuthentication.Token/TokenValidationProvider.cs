using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityAuthentication.Token
{
    internal class TokenValidationProvider : ITokenValidationProvider
    {
        private readonly IAuthenticationConfigurationProvider _configurationProvider;

        public TokenValidationProvider(IAuthenticationConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public JwtSecurityToken AccessTokenSecurity => throw new NotImplementedException();

        public JwtSecurityToken? RefreshTTokenSecurity => throw new NotImplementedException();

        public TokenValidationParameters AccessTokenValidationParameters => throw new NotImplementedException();

        public TokenValidationParameters? RefreshTokenValidationParameters => throw new NotImplementedException();
    }
}
