
namespace IdentityAuthentication.Configuration
{
    public class AesEncryptionConfiguration
    {
        public string SecurityKey { set; get; }

        public string Iv { set; get; }

        public const string ConfigurationKey = "TokenAesEncryption";
    }
}
