using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Model;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityAuthentication.Token
{
    internal class TokenSignatureProvider : ITokenSignatureProvider
    {
        private readonly IAuthenticationConfigurationProvider _configurationProvider;

        public TokenSignatureProvider(IAuthenticationConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        private TokenValidation AccessTokenValidation { set; get; }

        public JwtSecurityToken AccessTokenSecurity => throw new NotImplementedException();

        public JwtSecurityToken? RefreshTTokenSecurity => throw new NotImplementedException();

        public TokenValidationParameters AccessTokenValidationParameters => throw new NotImplementedException();

        public TokenValidationParameters? RefreshTokenValidationParameters => throw new NotImplementedException();
    }
}
