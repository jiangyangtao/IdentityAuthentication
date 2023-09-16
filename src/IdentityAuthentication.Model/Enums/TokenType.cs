using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Model.Enums
{
    public enum TokenType
    {
        /// <summary>
        /// Json Web Token
        /// </summary>
        JWT = 1,

        /// <summary>
        /// Reference Token
        /// </summary>
        Reference = 2,

        /// <summary>
        /// Encrypt Token
        /// </summary>
        Encrypt
    }
}
