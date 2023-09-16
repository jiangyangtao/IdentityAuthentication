using IdentityAuthentication.Model.Configurations;

namespace IdentityAuthentication.Configuration
{
    public class RsaSignatureConfiguration : RsaVerifySignature
    {
        public string PrivateKey { set; get; }
    }
}
