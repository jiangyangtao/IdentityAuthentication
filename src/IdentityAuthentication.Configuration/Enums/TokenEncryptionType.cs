
namespace IdentityAuthentication.Configuration.Enums
{
    public enum TokenEncryptionType
    {
        /// <summary>
        /// RSA 非对称加密
        /// </summary>
        Rsa,

        /// <summary>
        /// RSA 对称加密，推荐使用
        /// </summary>
        Aes,
    }
}
