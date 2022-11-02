using IdentityAuthentication.TokenServices.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.TokenServices.Providers
{
    internal class MemoryCacheProvider : ICacheProvider
    {

        private readonly IMemoryCache _memoryCache;

        private readonly MemoryCache _Cache;

        public MemoryCacheProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _Cache = new MemoryCache(new MemoryCacheOptions { SizeLimit = 1024 });
        }

        public StorageType StorageType => StorageType.Memory;
    }
}
