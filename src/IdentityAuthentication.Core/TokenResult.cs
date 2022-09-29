using IdentityAuthentication.Abstractions;

namespace IdentityAuthentication.Core
{
    internal class TokenResult : IToken
    {
        private TokenResult()
        {

        }

        public string Token { set; get; }

        public long ExpiresIn { set; get; }

        public string TokenType { set; get; }

        public static TokenResult CreateReulst(string token, long expiresIn, string tokenType) =>
            new()
            {
                Token = token,
                ExpiresIn = expiresIn,
                TokenType = tokenType
            };
    }
}
