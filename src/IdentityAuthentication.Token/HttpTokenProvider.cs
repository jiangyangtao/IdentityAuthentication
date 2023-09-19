using IdentityAuthentication.Model.Extensions;
using IdentityAuthentication.Token.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityAuthentication.Token
{
    internal class HttpTokenProvider : IHttpTokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpTokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? AccessToken => _httpContextAccessor?.HttpContext?.Request?.Headers?.GetAuthorization();

        public string? RefreshToken => _httpContextAccessor?.HttpContext?.Request?.Headers?.GetRefreshToken();

        public IEnumerable<Claim> UserClaims
        {
            get
            {
                var claims = _httpContextAccessor?.HttpContext?.User?.Claims;
                if (claims.IsNullOrEmpty()) return Array.Empty<Claim>();

                return claims;
            }
        }

        public TokenInfo? AccessTokenInfo
        {
            get
            {
                if (UserClaims.IsNullOrEmpty()) return null;

                var result = TokenInfo.TryParse(UserClaims, out TokenInfo? accessToken);
                if (result == false) return null;
                if (accessToken == null) return null;

                return accessToken;
            }
        }
    }
}
