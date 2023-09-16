using IdentityAuthentication.Configuration.Enums;

namespace IdentityAuthentication.Configuration
{
    public class TokenEncryptionConfiguration
    {
        public TokenEncryptionAlgorithmType AlgorithmType { set; get; }

        public RsaEncryptionConfiguration RsaEncryption { set; get; }

        public AesEncryptionConfiguration AesEncryption { set; get; }
    }
}
