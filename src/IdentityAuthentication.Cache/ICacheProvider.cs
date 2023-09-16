using IdentityAuthentication.Configuration;
using IdentityAuthentication.Configuration.Enums;


namespace IdentityAuthentication.Cache
{
    public interface ICacheProvider
    {
        CacheStorageType StorageType { get; }

        Task<ReferenceToken> GetAsync(string key);

        Task SetAsync(string key, ReferenceToken data);

        Task RemoveAsync(string key);
    }
}