using Authentication.Abstractions;
using Authentication.Abstractions.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Local
{
    internal class LocalUserAuthenticationService : IAuthenticationService<PasswordCredential>
    {
        public string AuthenticationSource => throw new NotImplementedException();

        public Task<IAuthenticationResult> Authenticate(PasswordCredential credential, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
