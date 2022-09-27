using IdentityAuthentication.Abstractions;

namespace IdentityAuthentication.Core
{
    internal class TokenInfo : IToken
    {
        public bool Success { set; get; }

        public string Token { set; get; }

        public double ExpiresIn { set; get; }

        public string TokenType { set; get; }
    }
}
