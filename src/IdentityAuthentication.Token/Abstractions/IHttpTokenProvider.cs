using IdentityAuthentication.Model.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityAuthentication.Token.Abstractions
{
    internal interface IHttpTokenProvider
    {
        string? AccessToken { get; }

        string? RefreshToken { get; }

        bool AccessTokenIsEmpty => AccessToken.IsNullOrEmpty();

        bool RefreshTokenIsEmpty => RefreshToken.IsNullOrEmpty();

        IEnumerable<Claim> UserClaims { get; }

        TokenInfo? AccessTokenInfo { get; }
    }
}
