using Authentication.Abstractions;
using Authentication.Abstractions.Credentials;
using IdentityAuthentication.Abstractions;
using Microsoft.Extensions.Options;
using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.LDAP
{
    internal class LdapAuthenticationService : IAuthenticationService<PasswordCredential>
    {
        private readonly LdapConfiguration ldapConfiguration;

        public LdapAuthenticationService(IOptions<LdapConfiguration> ldapConfigurationPptions)
        {
            ldapConfiguration = ldapConfigurationPptions.Value;
        }

        public string GrantSource => "LDAP";

        public Task<IAuthenticationResult> AuthenticateAsync(PasswordCredential credential)
        {
            var username = GetUsername(credential);
            using (var validateConnection = new LdapConnection())
            {
                validateConnection.Connect(ldapConfiguration.Endpoint, ldapConfiguration.Port);
                validateConnection.Bind($"{username}@{ldapConfiguration.Domain}", credential.Password);
                if (validateConnection.Bound == false)throw new Exception("validate failed.");
            }

            var metadata = new Dictionary<string, string> {
                {"Email","admin@abc.com" }
            };
            var id = Guid.NewGuid().ToString();
            var result = credential.CreateAuthenticationResult(id, metadata);
            return Task.FromResult(result);
        }

        public Task<bool> IdentityCheckAsync(string id, string username)
        {
            throw new NotImplementedException();
        }

        private string GetUsername(PasswordCredential credential)
        {
            var username = credential.Username;
            var index = username.IndexOf('@');
            if (index != -1) return username.Remove(index);

            return username;
        }
    }
}
