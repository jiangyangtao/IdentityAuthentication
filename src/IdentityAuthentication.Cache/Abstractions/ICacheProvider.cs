using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Configuration.Enums;

namespace IdentityAuthentication.Cache.Abstractions
{
    public interface ICacheProvider<T> where T : IAuthenticationResult, new()
    {
        CacheStorageType StorageType { get; }

        Task<T?> GetAsync(string key);

        Task SetAsync(string key, T data);

        Task RemoveAsync(string key);
    }
}