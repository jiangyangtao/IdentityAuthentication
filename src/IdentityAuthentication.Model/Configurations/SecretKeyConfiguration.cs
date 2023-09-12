

namespace IdentityAuthentication.Model.Configurations
{
    public class SecretKeyConfigurationBase
    {
        public SecretKeyConfigurationBase() { }

        public SecretKeyConfigurationBase(string hmacSha256Key, string rsaPublicKey)
        {
            HmacSha256Key = hmacSha256Key;
            RsaSignaturePublicKey = rsaPublicKey;
        }

        /// <summary>
        /// HmacSha256
        /// </summary>
        public string HmacSha256Key { set; get; }

        /// <summary>
        /// RSA 验签公钥
        /// </summary>
        public string RsaSignaturePublicKey { set; get; }
    }

    public class SecretKeyConfiguration : SecretKeyConfigurationBase
    {
        /// <summary>
        /// RSA 签名私钥
        /// </summary>
        public string RsaSignaturePrivateKey { set; get; }

        /// <summary>
        /// RSA 解密私钥
        /// </summary>
        public string RsaDecryptPrivateKey { set; get; }

        /// <summary>
        /// RSA 加密公钥
        /// </summary>
        public string RsaEncryptPublicKey { set; get; }
    }
}
