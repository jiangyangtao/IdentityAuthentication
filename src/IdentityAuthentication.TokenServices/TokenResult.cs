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

        public string TokenType { set; get; }

        public string RefreshToken { set; get; }

        public static TokenResult Create(string accessToken, string refreshToken = "", long expiresIn = 0, string tokenType = JwtBearerDefaults.AuthenticationScheme) =>
            new()
            {
                AccessToken = accessToken,
                ExpiresIn = expiresIn,
                TokenType = tokenType,
                RefreshToken = refreshToken,
            };
    }
}
