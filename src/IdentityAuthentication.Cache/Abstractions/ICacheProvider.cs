using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Configuration.Model;

namespace IdentityAuthentication.Cache.Abstractions
{
    public interface ICacheProvider
    {
        CacheStorageType StorageType { get; }

        Task<ReferenceToken> GetAsync(string key);

        Task SetAsync(string key, ReferenceToken data);

        Task RemoveAsync(string key);
    }
}