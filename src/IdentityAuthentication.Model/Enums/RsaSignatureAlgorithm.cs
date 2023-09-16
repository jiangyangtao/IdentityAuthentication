using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Model.Enums
{
    public enum RsaSignatureAlgorithm
    {
        /// <summary>
        /// RS256
        /// </summary>
        RsaSha256 = 1,

        /// <summary>
        /// RS384
        /// </summary>
        RsaSha384 = 2,

        /// <summary>
        /// RS512
        /// </summary>
        RsaSha512 = 3,
    }
}
