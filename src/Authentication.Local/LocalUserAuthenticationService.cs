using Authentication.Abstractions;
using Authentication.Abstractions.Credentials;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Authentication.Local
{
    internal class LocalUserAuthenticationService : IAuthenticationService<PasswordCredential>
    {
        private readonly ILogger<LocalUserAuthenticationService> _logger;

        public LocalUserAuthenticationService(ILogger<LocalUserAuthenticationService> logger)
        {
            _logger = logger;
        }

        public string AuthenticationSource => "Local";

        public Task<AuthenticationResult> AuthenticateAsync(PasswordCredential credential)
        {
            var json = JsonConvert.SerializeObject(credential);
            _logger.LogInformation(json);

            var metadata = new Dictionary<string, string> {
                {"Email","admin@abc.com" }
            };
            var id = Guid.NewGuid().ToString();
            var result = credential.CreateAuthenticationResult(id, metadata);
            return Task.FromResult(result);
        }
    }
}
