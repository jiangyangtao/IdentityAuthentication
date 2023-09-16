using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Model.Configurations
{
    public class RsaVerifySignature : RsaSignatureBase
    {
        public string PublicKey { set; get; }
    }
}
