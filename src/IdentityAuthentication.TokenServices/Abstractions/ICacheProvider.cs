

namespace IdentityAuthentication.TokenServices.Abstractions
{
    internal interface ICacheProvider
    {
        public StorageType StorageType { get; }
    }
}
