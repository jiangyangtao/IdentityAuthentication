

namespace IdentityAuthentication.TokenServices
{
    internal class CacheStorageConfiguration
    {
        public StorageType StorageType { set; get; }

        public int MemonySizeLimit { set; get; }

        public int CacheExpirationTime { set; get; }

        public string RedisConnection { set; get; }
    }

    public enum StorageType
    {
        Memory = 1,
        Redis = 2
    }
}
