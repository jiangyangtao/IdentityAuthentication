using IdentityAuthentication.Configuration.Enums;
using IdentityAuthentication.Model.Configurations;

namespace IdentityAuthentication.Configuration
{
    public class AuthenticationConfiguration : AuthenticationConfigurationBase
    {    
        public TokenSignatureType TokenSignatureType { set; get; }

        public TokenEncryptionType TokenEncryptionType { set; get; }

        public const string ConfigurationKey = "Authentication";
    }
}
