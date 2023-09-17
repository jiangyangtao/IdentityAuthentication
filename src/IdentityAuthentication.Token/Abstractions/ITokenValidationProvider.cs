using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityAuthentication.Token.Abstractions
{
    internal interface ITokenValidationProvider
    {
        JwtSecurityToken AccessTokenSecurity { get; }

        JwtSecurityToken? RefreshTTokenSecurity { get; }

        TokenValidationParameters AccessTokenValidationParameters { get; }

        TokenValidationParameters? RefreshTokenValidationParameters { get; }
    }
}
