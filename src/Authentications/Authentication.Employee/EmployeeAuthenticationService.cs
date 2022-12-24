using Authentication.Abstractions;
using Authentication.Abstractions.Credentials;

namespace Authentication.Employee
{
    internal class EmployeeAuthenticationService : IAuthenticationService<PasswordCredential>
    {
        public string GrantSource => "Employee";

        public Task<AuthenticationResult> AuthenticateAsync(PasswordCredential credential)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IdentityCheckAsync(string id, string username)
        {
            throw new NotImplementedException();
        }
    }
}
