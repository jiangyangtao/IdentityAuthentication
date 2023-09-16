
namespace IdentityAuthentication.Model.Configurations
{
    /// <summary>
    /// Rsa 验签配置
    /// </summary>
    public class RsaVerifySignatureConfiguration : RsaBase
    {
        public string PublicKey { set; get; }

        public RsaSignature ToRsaSignature()
        {
            return new()
            {
                RSAKeyType = RSAKeyType,
                RsaSignatureAlgorithm = RsaSignatureAlgorithm,
                SignatureKey = PublicKey,
            };
        }
    }
}
