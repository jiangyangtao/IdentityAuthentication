using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.TokenServices.Abstractions
{
    internal interface ICacheProvider
    {
        public StorageType StorageType { get; }
    }
}
