
namespace IdentityAuthentication.Model
{
    public class AuthenticationEndpoints
    {
        public string AutnenticationConfigurationEndpoint { set; get; }

        public string GenerateTokenEndpoint { set; get; }

        public string RefreshToeknEndpoint { set; get; }

        public string AuthorizeEndpoint { set; get; }

        public static string DefaultConfigurationEndpoint => "/api/configuration";

    }
}
