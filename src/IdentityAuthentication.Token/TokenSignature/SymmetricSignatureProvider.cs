using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.Token.TokenSignature
{
    internal class SymmetricSignatureProvider : ITokenSignatureProvider
    {
        public SymmetricSignatureProvider()
        {
        }

        public TokenSignatureType TokenSignatureType => throw new NotImplementedException();

        public string BuildAccessToken(TokenInfo tokenInfo)
        {
            throw new NotImplementedException();
        }

        public string BuildRefreshToken(TokenInfo refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<TokenValidationResult> ValidateAccessTokenAsync(string token)
        {
            throw new NotImplementedException();
        }

        public Task<TokenValidationResult?> ValidateRefreshTokenAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}
