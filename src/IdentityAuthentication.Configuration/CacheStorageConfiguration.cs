using IdentityAuthentication.Configuration.Enums;

namespace IdentityAuthentication.Configuration
{
    public class CacheStorageConfiguration
    {
        public CacheStorageType StorageType { set; get; }

        public int MemonySizeLimit { set; get; }

        public int CacheExpirationTime { set; get; }

        public string RedisConnection { set; get; }
    }
}
