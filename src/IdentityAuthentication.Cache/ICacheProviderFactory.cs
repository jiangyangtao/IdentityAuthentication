

namespace IdentityAuthentication.Cache
{
    internal interface ICacheProviderFactory
    {
        public ICacheProvider CreateCacheProvider();
    }
}
