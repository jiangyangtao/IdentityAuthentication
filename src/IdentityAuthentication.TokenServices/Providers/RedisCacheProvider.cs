using IdentityAuthentication.TokenServices.Abstractions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace IdentityAuthentication.TokenServices.Providers
{
    internal class RedisCacheProvider : ICacheProvider
    {
        private readonly IDatabase _database;
        private readonly CacheStorageConfiguration _cacheStorageConfiguration;

        public RedisCacheProvider(IOptions<CacheStorageConfiguration> options)
        {
            _cacheStorageConfiguration = options.Value;

            var configurationOptions = ConfigurationOptions.Parse(_cacheStorageConfiguration.RedisConnection, true);
            _database = ConnectionMultiplexer.Connect(configurationOptions).GetDatabase();
        }


        public StorageType StorageType => StorageType.Redis;

        public async Task<ReferenceToken> GetAsync(string key)
        {
            var json = await _database.StringGetAsync(key);
            if (json.IsNullOrEmpty) return null;

            var obj = JObject.Parse(json.ToString());
            return obj.ToObject<ReferenceToken>();
        }

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }

        public async Task SetAsync(string key, ReferenceToken data)
        {
            var json = JsonConvert.SerializeObject(data);
            await _database.StringSetAsync(key, json, TimeSpan.FromDays(_cacheStorageConfiguration.CacheExpirationTime), When.Always);
        }
    }
}
