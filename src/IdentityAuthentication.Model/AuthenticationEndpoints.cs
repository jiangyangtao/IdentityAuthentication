
namespace IdentityAuthentication.Model
{
    public class AuthenticationEndpoints
    {
        /// <summary>
        /// Get access token config the endpoint
        /// </summary>
        public string AccessTokenConfigurationEndpoint { set; get; }

        /// <summary>
        /// Get refresh token config the endpoint
        /// </summary>
        public string RefreshTokenConfigurationEndpoint { set; get; }

        public string SecretKeyConfigurationEndpoint { set; get; }

        public string AutnenticationConfigurationEndpoint { set; get; }

        public string GenerateTokenEndpoint { set; get; }

        public string RefreshToeknEndpoint { set; get; }

        public string AuthorizeEndpoint { set; get; }

        public string InfoEndpoint { set; get; }

        public static string DefaultConfigurationEndpoint => "/api/configuration";

    }
}
