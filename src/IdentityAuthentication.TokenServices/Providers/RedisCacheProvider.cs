using IdentityAuthentication.TokenServices.Abstractions;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.TokenServices.Providers
{
    internal class RedisCacheProvider : ICacheProvider
    {
        private readonly IDatabase _database;

        public RedisCacheProvider(IOptions<CacheStorageConfiguration> options)
        {
            var configurationOptions = ConfigurationOptions.Parse(options.Value.RedisConnection, true);
            _database = ConnectionMultiplexer.Connect(configurationOptions).GetDatabase();
        }


        public StorageType StorageType => StorageType.Redis;
    }
}
