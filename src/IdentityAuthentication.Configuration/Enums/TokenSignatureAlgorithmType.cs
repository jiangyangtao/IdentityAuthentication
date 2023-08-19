
namespace IdentityAuthentication.Configuration.Enums
{
    /// <summary>
    /// Jwt 签名算法
    /// </summary>
    public enum TokenSignatureAlgorithmType
    {
  

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
