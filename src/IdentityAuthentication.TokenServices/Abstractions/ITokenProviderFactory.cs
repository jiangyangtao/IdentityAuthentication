

namespace IdentityAuthentication.TokenServices.Abstractions
{
    public interface ITokenProviderFactory
    {
        public ITokenProvider CreateTokenService();
    }
}
