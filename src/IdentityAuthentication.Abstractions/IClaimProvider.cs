using System.Security.Claims;

namespace IdentityAuthentication.Abstractions
{
    public interface IClaimProvider
    {
        Claim[] BuildClaim(string id, string username, IReadOnlyDictionary<string, string> metadata);

        Claim[] ResetTokenExpiration();
    }
}
