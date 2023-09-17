
namespace IdentityAuthentication.Configuration.Enums
{
    /// <summary>
    /// Jwt 签名算法
    /// </summary>
    public enum TokenSignatureAlgorithmType
    {
        /// <summary>
        /// HS256
        /// </summary>
        HmacSha256 = 10,

        /// <summary>
        /// HS384
        /// </summary>
        HmacSha384 = 11,

        /// <summary>
        /// HS512
        /// </summary>
        HmacSha512 = 12,

        /// <summary>
        /// RS256
        /// </summary>
        RsaSha256 = 20,

        /// <summary>
        /// RS384
        /// </summary>
        RsaSha384 = 21,

        /// <summary>
        /// RS512
        /// </summary>
        RsaSha512 = 22,
    }
}
