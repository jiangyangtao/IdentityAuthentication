using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Extensions
{
    public static class IdentityExtensions
    {
        public static ClaimsPrincipal ToClaimsPrincipal(this IIdentity identity)
        {
            return new ClaimsPrincipal(identity);
        }
    }
}
