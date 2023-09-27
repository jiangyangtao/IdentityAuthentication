using IdentityAuthentication.Configuration.Enums;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.Token.Abstractions
{
    internal interface ITokenSignatureProvider
    {
        TokenSignatureType TokenSignatureType { get; }

        string BuildAccessToken(TokenInfo tokenInfo);

        Task<TokenValidationResult> ValidateAccessTokenAsync(string token);

        string BuildRefreshToken(TokenInfo refreshToken);

        Task<TokenValidationResult?> ValidateRefreshTokenAsync(string token);
    }
}
