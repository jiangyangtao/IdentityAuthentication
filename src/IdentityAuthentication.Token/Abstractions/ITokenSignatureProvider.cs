using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityAuthentication.Token.Abstractions
{
    internal interface ITokenSignatureProvider
    {
        JwtSecurityToken GenerateAccessSecurityToken(Claim[] claims, DateTime notBefore, DateTime expirationTime);

        TokenValidationParameters GenerateAccessTokenValidation();

        JwtSecurityToken? GenerateRefreshSecurityToken(Claim[] claims, DateTime notBefore, DateTime expirationTime);

        TokenValidationParameters? GenerateRefreshTokenValidation();
    }
}
