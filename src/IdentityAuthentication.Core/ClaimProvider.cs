using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace IdentityAuthentication.Core
{
    internal class ClaimProvider : IClaimProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationConfig authenticationConfig;

        public ClaimProvider(IOptions<AuthenticationConfig> options, IHttpContextAccessor httpContextAccessor)
        {
            authenticationConfig = options.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        private DateTime TokenExpirationTime => DateTime.Now.AddSeconds(authenticationConfig.TokenExpirationTime);

        public Claim[] BuildClaim(string id, string username, IReadOnlyDictionary<string, string> metadata)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, id),
                new Claim(ClaimTypes.Name,username),
                new Claim(ClaimTypes.Expiration, TokenExpirationTime.ToString())
            };

            foreach (var item in metadata)
            {
                claims.Add(new Claim(item.Key, item.Value));
            }
            return claims.ToArray();
        }

        private void CheckTokenImmediatelyExpire()
        {
            if (Claims.IsNullOrEmpty()) throw new Exception("Authentication failed");

            var result = Claims.FirstOrDefault(a => a.Type == ClaimTypes.Expiration);
            if (result == null) throw new Exception("Authentication failed");

            var parseResult = DateTime.TryParse(result.Value, out DateTime expirationTime);
            if (parseResult == false) throw new Exception("Authentication failed");

            var timeSpan = expirationTime - DateTime.Now;
            if (timeSpan.TotalSeconds > authenticationConfig.TokenRefreshTime) throw new Exception("Token do not expire immediately");
        }

        /// <summary>
        /// 重置 token 的过期时间
        /// </summary>
        /// <returns></returns>
        public Claim[] ResetTokenExpiration()
        {
            CheckTokenImmediatelyExpire();

            var claims = new List<Claim>();
            foreach (var item in Claims)
            {
                var claim = new Claim(item.Type, item.Value);
                if (item.Type == ClaimTypes.Expiration)
                {
                    claim = new Claim(item.Type, TokenExpirationTime.ToString());
                }

                claims.Add(claim);
            }

            return claims.ToArray();
        }

        private IReadOnlyCollection<Claim> Claims
        {
            get
            {
                if (_httpContextAccessor == null) return Array.Empty<Claim>();
                if (_httpContextAccessor.HttpContext == null) return Array.Empty<Claim>();
                if (_httpContextAccessor.HttpContext.User == null) return Array.Empty<Claim>();

                return _httpContextAccessor.HttpContext.User.Claims.ToArray();
            }
        }
    }
}
