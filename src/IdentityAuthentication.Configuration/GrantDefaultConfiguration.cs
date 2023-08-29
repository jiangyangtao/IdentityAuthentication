namespace IdentityAuthentication.Configuration
{
    public class GrantDefaultConfiguration
    {
        public string GrantTypeDefault { set; get; }

        public string GrantSourceDefault { set; get; }

        public string ClientDefault { set; get; }

        public const string ConfigurationKey = "GrantDefaults";
    }
}