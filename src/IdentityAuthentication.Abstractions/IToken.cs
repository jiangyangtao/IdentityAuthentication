
namespace IdentityAuthentication.Abstractions
{
    public interface IToken
    {
        public string Token { get; }

        public long ExpiresIn { get; }

        public string TokenType { get; }
    }
}
