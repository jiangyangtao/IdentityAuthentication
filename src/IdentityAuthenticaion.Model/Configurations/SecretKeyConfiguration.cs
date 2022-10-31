using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthenticaion.Model.Configurations
{
    public class SecretKeyConfiguration
    {
        /// <summary>
        /// HmacSha256
        /// </summary>
        public string HmacSha256Key { set; get; }

        public string RsaPublicKey { set; get; }

        public string RsaPrivateKey { set; get; }
    }
}
