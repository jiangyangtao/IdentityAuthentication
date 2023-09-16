using IdentityAuthentication.Configuration.Enums;
using System.Security.Cryptography;

namespace IdentityAuthentication.Configuration
{
    public class RsaEncryptionConfiguration : RsaSignatureConfiguration
    {
        public RsaSignaturePaddingType SignaturePadding { set; get; }

        public RSAEncryptionPadding RSAEncryptionPadding
        {
            get
            {
                if (SignaturePadding == RsaSignaturePaddingType.Pkcs1) return RSAEncryptionPadding.Pkcs1;
                if (SignaturePadding == RsaSignaturePaddingType.OaepSHA1) return RSAEncryptionPadding.OaepSHA1;
                if (SignaturePadding == RsaSignaturePaddingType.OaepSHA256) return RSAEncryptionPadding.OaepSHA256;
                if (SignaturePadding == RsaSignaturePaddingType.OaepSHA384) return RSAEncryptionPadding.OaepSHA384;

                return RSAEncryptionPadding.OaepSHA512;
            }
        }
    }
}
