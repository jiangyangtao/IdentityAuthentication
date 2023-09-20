namespace IdentityAuthentication.Token.Abstractions
{
    public interface ITokenProviderFactory
    {
        public ITokenProvider CreateTokenService();
    }
}
