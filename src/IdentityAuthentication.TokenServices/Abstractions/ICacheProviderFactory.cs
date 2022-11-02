

namespace IdentityAuthentication.TokenServices.Abstractions
{
    internal interface ICacheProviderFactory
    {
        public ICacheProvider CreateCacheProvider();
    }
}
