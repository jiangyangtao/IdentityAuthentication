namespace IdentityAuthentication.Cache.Abstractions
{
    public interface ICacheProviderFactory
    {
        public ICacheProvider CreateCacheProvider();
    }
}
