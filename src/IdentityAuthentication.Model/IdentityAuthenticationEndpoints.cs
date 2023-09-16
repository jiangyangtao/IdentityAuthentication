
namespace IdentityAuthentication.Model
{
    public class IdentityAuthenticationEndpoints
    {
        public string AuthenticationConfigurationEndpoint { set; get; }

        public string GenerateTokenEndpoint { set; get; }

        public string RefreshToeknEndpoint { set; get; }

        public string AuthorizeEndpoint { set; get; }

        public static string DefaultConfigurationEndpoint => "/api/v1/configuration/authenticationendpoints";

    }
}
