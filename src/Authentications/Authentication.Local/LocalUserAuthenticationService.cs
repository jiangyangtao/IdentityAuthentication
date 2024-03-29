﻿using Authentication.Abstractions;
using Authentication.Abstractions.Credentials;
using IdentityAuthentication.Abstractions;
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

        public string GrantSource => "Local";

        public Task<IAuthenticationResult> AuthenticateAsync(PasswordCredential credential)
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

        public Task<bool> IdentityValidationAsync(string id, string username)
        {
            return Task.FromResult(true);
        }
    }
}
