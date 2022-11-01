using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.TokenServices.Abstractions
{
    internal interface ICacheProviderFactory
    {
        public ICacheProvider CreateCacheProvider(StorageType storageType);
    }
}
