using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Model.Configurations
{
    public class SecretKeyConfigurationBase
    {
        public SecretKeyConfigurationBase() { }

        public SecretKeyConfigurationBase(string hmacSha256Key, string rsaPublicKey)
        {
            HmacSha256Key = hmacSha256Key;
            RsaPublicKey = rsaPublicKey;
        }


        /// <summary>
        /// HmacSha256
        /// </summary>
        public string HmacSha256Key { set; get; }

        public string RsaPublicKey { set; get; }
    }

    public class SecretKeyConfiguration : SecretKeyConfigurationBase
    {
        public string RsaPrivateKey { set; get; }
    }
}
