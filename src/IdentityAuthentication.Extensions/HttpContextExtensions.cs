using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace IdentityAuthentication.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetRefreshToken(this HttpContext context)
        {
            if (context == null) return string.Empty;

            var r = context.Request.Headers.TryGetValue("refresh-token", out StringValues token);
            if (r == false) return string.Empty;

            return token;
        }
    }
}
