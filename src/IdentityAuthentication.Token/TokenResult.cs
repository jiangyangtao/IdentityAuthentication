using IdentityAuthentication.Configuration.Model;

namespace IdentityAuthentication.Token
{
    internal class TokenResult : ITokenResult
    {
        private TokenResult(string accessToken, long expiresIn, IReadOnlyDictionary<string, string> userInfo)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
            UserInfo = userInfo;
        }

        private TokenResult(string accessToken, long expiresIn, IReadOnlyDictionary<string, string> userInfo, string refreshToken) : this(accessToken, expiresIn, userInfo)
        {
            RefreshToken = refreshToken;
        }

        public string AccessToken { set; get; }

        public long ExpiresIn { set; get; }

        public IReadOnlyDictionary<string, string> UserInfo { set; get; }

        public string RefreshToken { set; get; }

        public static IToken CreateToken(string accessToken, long expiresIn, IReadOnlyDictionary<string, string> userInfo)
            => new TokenResult(accessToken, expiresIn, userInfo);

        public static ITokenResult CreateToken(string accessToken, long expiresIn, IReadOnlyDictionary<string, string> userInfo, string refreshToken)
           => new TokenResult(accessToken, expiresIn, userInfo, refreshToken);
    }
}
