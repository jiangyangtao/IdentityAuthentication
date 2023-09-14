
namespace IdentityAuthentication.Core
{
    public interface IToken
    {
        public string AccessToken { get; }

        public long ExpiresIn { get; }

        public string TokenType { get; }

        public IReadOnlyDictionary<string, string> UserInfo { get; }
    }

    public interface IRefreshToken
    {
        public string RefreshToken { set; get; }
    }

    public interface ITokenResult : IToken, IRefreshToken
    {

    }
}
