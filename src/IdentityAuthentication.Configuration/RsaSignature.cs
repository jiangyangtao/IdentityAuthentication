using IdentityAuthentication.Model.Configurations;

namespace IdentityAuthentication.Configuration
{
    public class RsaSignature : RsaVerifySignature
    {
        public string PrivateKey { set; get; }
    }
}
