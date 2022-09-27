
namespace IdentityAuthentication.Abstractions
{
    public interface IToken
    {
        public bool Success { get; }

        public string Token { get; }

        public double ExpiresIn { get; }

        public string TokenType { get; }
    }
}
