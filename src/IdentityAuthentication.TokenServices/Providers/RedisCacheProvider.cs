using IdentityAuthentication.Model.Configurations;
using IdentityAuthentication.TokenServices.Abstractions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace IdentityAuthentication.TokenServices.Providers
{
    internal class RedisCacheProvider : ICacheProvider
    {
        private const string _tokenContainerName = "Token:";
        private const string _userTokenContainerName = "UserToken:";
        private readonly IDatabase _database;
        private readonly CacheStorageConfiguration _cacheStorageConfiguration;

        public RedisCacheProvider(IOptions<AuthenticationConfigurationBase> authenticationConfig,
            IOptions<CacheStorageConfiguration> cacheStorageConfig)
        {
            _cacheStorageConfiguration = cacheStorageConfig.Value;

            if (authenticationConfig.Value.TokenType == TokenType.Reference && _cacheStorageConfiguration.StorageType == StorageType.Redis)
            {
                var configurationOptions = ConfigurationOptions.Parse(_cacheStorageConfiguration.RedisConnection, true);
                _database = ConnectionMultiplexer.Connect(configurationOptions).GetDatabase();
            }
        }


        public StorageType StorageType => StorageType.Redis;

        private string BuildTokenKey(string key) => $"{_tokenContainerName}{key}";

        private string BuildUserTokenKey(ReferenceToken data) => $"{_userTokenContainerName}{data.UserId}:{data.Client}";


        public async Task<ReferenceToken> GetAsync(string key)
        {
            key = BuildTokenKey(key);
            var json = await _database.StringGetAsync(key);
            if (json.IsNullOrEmpty) return null;

            var obj = JObject.Parse(json.ToString());
            return obj.ToObject<ReferenceToken>();
        }

        public async Task RemoveAsync(string key)
        {
            key = BuildTokenKey(key);
            await _database.KeyDeleteAsync(key);
        }

        public async Task SetAsync(string key, ReferenceToken data)
        {
            key = BuildTokenKey(key);
            var json = JsonConvert.SerializeObject(data);
            await _database.StringSetAsync(key, json, TimeSpan.FromDays(_cacheStorageConfiguration.CacheExpirationTime), When.Always);
            await SetUserTokenAsync(data, key);
        }

        /// <summary>
        /// 保存用户 Id 与 token 的对应关系
        /// </summary>
        /// <param name="token"></param>
        /// <param name="tokenKey"></param>
        /// <returns></returns>
        private async Task SetUserTokenAsync(ReferenceToken token, string tokenKey)
        {
            var key = BuildUserTokenKey(token);
            tokenKey = tokenKey["Bearer ".Length..].Trim();
            await _database.StringSetAsync(key, tokenKey, TimeSpan.FromDays(90), When.Always);
        }
    }
}
