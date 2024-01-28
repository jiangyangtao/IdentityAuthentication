using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace IdentityAuthentication.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetRefreshToken(this HttpRequest request)
        {
            if (request == null) return string.Empty;

            var r = request.Headers.TryGetValue("refresh-token", out StringValues token);
            if (r == false) return string.Empty;

            return token;
        }

        public static string GetOriginHost(this HttpRequest request)
        {
            if (request == null) return string.Empty;

            var r = request.Headers.TryGetValue("X-Forwarded-Proto", out StringValues schemeValues);
            var scheme = r ? schemeValues.ToString() : request.Scheme;
            return $"{scheme}://{request.Host}";
        }
    }
}
