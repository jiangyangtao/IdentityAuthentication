using IdentityAuthentication.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IdentityAuthentication.TokenServices
{
    internal class TokenResult : IToken
    {
        private TokenResult()
        {

        }

        public string AccessToken { set; get; }

        public long ExpiresIn { set; get; }

        public string TokenType { set; get; } = JwtBearerDefaults.AuthenticationScheme;

        public string RefreshToken { set; get; }

        public IReadOnlyDictionary<string, string> UserInfo { set; get; }

        public static TokenResult Create(string accessToken, IReadOnlyDictionary<string, string> userInfo, string refreshToken = "", long expiresIn = 0) =>
            new()
            {
                AccessToken = accessToken,
                ExpiresIn = expiresIn,
                RefreshToken = refreshToken,
            };
    }
}
