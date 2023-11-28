using Authentication.Abstractions.Credentials;
using Authentication.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityAuthentication.Abstractions;

namespace Authentication.Firm
{
    public class FirmAuthenticationService : IAuthenticationService<PasswordCredential>
    {
        public FirmAuthenticationService()
        {
        }

        public string GrantSource => "Firm";

        public Task<IAuthenticationResult> AuthenticateAsync(PasswordCredential credential)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IdentityValidationAsync(string id, string username)
        {
            throw new NotImplementedException();
        }
    }
}
