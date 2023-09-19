﻿using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityAuthentication.Token.Abstractions
{
    internal interface ITokenSignatureProvider
    {
        string BuildAccessToken(TokenInfo tokenInfo);

        Task<TokenValidationResult> ValidateAccessTokenAsync(string token);

        string BuildRefreshToken(Claim[] claims);

        Task<TokenValidationResult?> ValidateRefreshTokenAsync(string token);
    }
}
