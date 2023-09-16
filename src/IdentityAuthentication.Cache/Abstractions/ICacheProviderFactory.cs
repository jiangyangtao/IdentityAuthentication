namespace IdentityAuthentication.Cache.Abstractions
{
    internal interface ICacheProviderFactory
    {
        public ICacheProvider CreateCacheProvider();
    }
}
