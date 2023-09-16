using IdentityAuthentication.Model.Configurations;

namespace IdentityAuthentication.Configuration
{
    public class RsaSignatureConfiguration : RsaVerifySignatureConfiguration
    {
        public string PrivateKey { set; get; }
    }
}
