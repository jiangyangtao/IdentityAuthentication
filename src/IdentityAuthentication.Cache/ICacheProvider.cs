using IdentityAuthentication.Configuration.Enums;


namespace IdentityAuthentication.Cache
{
    public interface ICacheProvider<TToken>
    {
        CacheStorageType StorageType { get; }

        Task<TToken> GetAsync(string key);

        Task SetAsync(string key, TToken data);

        Task RemoveAsync(string key);
    }
}