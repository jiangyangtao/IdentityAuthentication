using IdentityAuthentication.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Model.Configurations
{
    public abstract class RsaSignatureBase
    {
        public RsaSignatureAlgorithm RsaSignatureAlgorithm { set; get; }
    }
}
