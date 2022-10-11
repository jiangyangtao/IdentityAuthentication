using IdentityAuthentication.Abstractions;

namespace IdentityAuthentication.Core
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

        public static TokenResult CreateReulst(string accessToken, long expiresIn, string tokenType, string refreshToken) =>
            new()
            {
                AccessToken = accessToken,
                ExpiresIn = expiresIn,
                TokenType = tokenType,
                RefreshToken = refreshToken,
            };
    }
}
