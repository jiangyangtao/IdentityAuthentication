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
            if (configurationProvider.AccessToken == null) throw new ArgumentException(nameof(configurationProvider.AccessToken));
            _configurationProvider = configurationProvider;
        }

        private TokenValidation AccessTokenSignature { set; get; }

        public JwtSecurityToken AccessTokenSecurity
        {
            get
            {

            }
        }


        private TokenValidation AccessTokenValidation { set; get; }

        public TokenValidationParameters AccessTokenValidationParameters => throw new NotImplementedException();




        private TokenValidation RefreshTokenSignature { set; get; }

        public JwtSecurityToken? RefreshTokenSecurity => throw new NotImplementedException();

        private TokenValidation RefreshTokenValidation { set; get; }

        public TokenValidationParameters? RefreshTokenValidationParameters => throw new NotImplementedException();
    }
}
