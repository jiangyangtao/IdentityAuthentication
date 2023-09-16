using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityAuthentication.Model.Enums
{
    /// <summary>
    /// Jwt 签名算法
    /// </summary>
    public enum JwtSignatureAlgorithm
    {
        /// <summary>
        /// HS256
        /// </summary>
        HmacSha256,

        /// <summary>
        /// HS384
        /// </summary>
        HmacSha384,

        /// <summary>
        /// HS512
        /// </summary>
        HmacSha512,

        /// <summary>
        /// RS256
        /// </summary>
        RsaSha256,

        /// <summary>
        /// RS384
        /// </summary>
        RsaSha384,

        /// <summary>
        /// RS512
        /// </summary>
        RsaSha512,

        /// <summary>
        /// A128CBC-HS256
        /// </summary>
        Aes128CbcHmacSha256,

        /// <summary>
        /// A192CBC-HS384
        /// </summary>
        Aes192CbcHmacSha384,

        /// <summary>
        /// A256CBC-HS512
        /// </summary>
        Aes256CbcHmacSha512,
    }
}
