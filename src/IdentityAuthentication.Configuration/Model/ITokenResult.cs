namespace IdentityAuthentication.Configuration.Model
{
    public interface IToken
    {
        public string AccessToken { get; }

        public long ExpiresIn { get; }

        public IReadOnlyDictionary<string, string> UserInfo { get; }

        public string RefreshToken { get; }
    }
}
