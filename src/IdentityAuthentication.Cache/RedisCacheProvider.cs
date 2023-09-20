using IdentityAuthentication.Cache.Abstractions;
using IdentityAuthentication.Configuration;
using IdentityAuthentication.Configuration.Abstractions;
using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Model.Enums;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace IdentityAuthentication.Cache
{
    internal class RedisCacheProvider : ICacheProvider
    {
        private const string _tokenContainerName = "Token:";
        private const string _userTokenContainerName = "UserToken:";
        private readonly IDatabase _database;
        private readonly CacheStorageConfiguration _cacheStorageConfiguration;

        public RedisCacheProvider(IOptions<AuthenticationConfiguration> authenticationConfig,
            IOptions<CacheStorageConfiguration> cacheStorageConfig)
        {
            _cacheStorageConfiguration = cacheStorageConfig.Value;

            if (authenticationConfig.Value.TokenType == TokenType.Reference && _cacheStorageConfiguration.StorageType == CacheStorageType.Redis)
            {
                var configurationOptions = ConfigurationOptions.Parse(_cacheStorageConfiguration.RedisConnection, true);
                _database = ConnectionMultiplexer.Connect(configurationOptions).GetDatabase();
            }
        }


        public CacheStorageType StorageType => CacheStorageType.Redis;

        private static string BuildTokenKey(string key) => $"{_tokenContainerName}{key}";

        public async Task<T?> GetAsync<T>(string key) where T : IAuthenticationResult, new()
        {
            key = BuildTokenKey(key);
            var json = await _database.StringGetAsync(key);
            if (json.IsNullOrEmpty) return default;

            return JsonConvert.DeserializeObject<T>(json.ToString());
        }

        public async Task RemoveAsync(string key)
        {
            key = BuildTokenKey(key);
            await _database.KeyDeleteAsync(key);
        }

        public async Task SetAsync<T>(string key, T data) where T : IAuthenticationResult, new()
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
        private async Task SetUserTokenAsync<T>(T token, string tokenKey) where T : IAuthenticationResult, new()
        {
            var key = $"{_userTokenContainerName}{token.UserId}:{token.Client}";
            tokenKey = tokenKey["Bearer ".Length..].Trim();
            await _database.StringSetAsync(key, tokenKey, TimeSpan.FromDays(90), When.Always);
        }
    }
}
