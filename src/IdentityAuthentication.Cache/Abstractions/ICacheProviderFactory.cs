using IdentityAuthentication.Configuration.Abstractions;

namespace IdentityAuthentication.Cache.Abstractions
{
    public interface ICacheProviderFactory<T> where T : IAuthenticationResult, new()
    {
        public ICacheProvider<T> CreateCacheProvider();
    }
}
