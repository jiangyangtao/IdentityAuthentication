

using Newtonsoft.Json.Linq;

namespace IdentityAuthentication.TokenServices.Abstractions
{
    internal interface ICacheProvider
    {
        StorageType StorageType { get; }

        Task<ReferenceToken> GetAsync(string key);

        Task SetAsync(string key, ReferenceToken data);

        Task RemoveAsync(string key);
    }
}
