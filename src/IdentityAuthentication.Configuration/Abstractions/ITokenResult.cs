
namespace IdentityAuthentication.Configuration.Abstractions
{
    public interface IToken
    {
        public string AccessToken { get; }

        public long ExpiresIn { get; }

        public IReadOnlyDictionary<string, string> UserInfo { get; }
    }

    public interface IRefreshToken
    {
        public string RefreshToken { get; }
    }

    public interface ITokenResult : IToken, IRefreshToken
    {

    }
}
