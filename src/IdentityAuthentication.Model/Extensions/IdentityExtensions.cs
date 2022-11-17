using System.Security.Claims;
using System.Security.Principal;

namespace IdentityAuthentication.Model.Extensions
{
    public static class IdentityExtensions
    {
        public static ClaimsPrincipal ToClaimsPrincipal(this IIdentity identity) => new ClaimsPrincipal(identity);
    }
}
