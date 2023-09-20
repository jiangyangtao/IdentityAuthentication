using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Configuration.Enums;

namespace IdentityAuthentication.Cache.Abstractions
{
    public interface ICacheProvider
    {
        CacheStorageType StorageType { get; }

        Task<T?> GetAsync<T>(string key) where T : IAuthenticationResult, new();

        Task SetAsync<T>(string key, T data) where T : IAuthenticationResult, new();

        Task RemoveAsync(string key);
    }
}