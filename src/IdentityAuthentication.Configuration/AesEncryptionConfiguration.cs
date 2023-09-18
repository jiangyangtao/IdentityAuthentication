
using System.Text;

namespace IdentityAuthentication.Configuration
{
    public class AesEncryptionConfiguration
    {
        public string SecurityKey { set; get; }

        public string Iv { set; get; }

        public byte[] AesIV => Encoding.UTF8.GetBytes(Iv);

        public byte[] AesKey => Encoding.UTF8.GetBytes(SecurityKey);

        public const string ConfigurationKey = "TokenAesEncryption";
    }
}
