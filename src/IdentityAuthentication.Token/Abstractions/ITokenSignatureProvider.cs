using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityAuthentication.Token.Abstractions
{
    internal interface ITokenSignatureProvider
    {
        JwtSecurityToken AccessTokenSecurity { get; }

        JwtSecurityToken? RefreshTokenSecurity { get; }

        TokenValidationParameters AccessTokenValidationParameters { get; }

        TokenValidationParameters? RefreshTokenValidationParameters { get; }
    }
}
