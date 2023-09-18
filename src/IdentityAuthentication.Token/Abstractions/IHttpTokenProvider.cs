using System.Security.Claims;

namespace IdentityAuthentication.Token.Abstractions
{
    internal interface IHttpTokenProvider
    {
        string? AccessToken { get; }

        string? RefreshToken { get; }

        IEnumerable<Claim> UserClaims { get; }
    }
}
