
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
    }
}
