using Authentication.Abstractions;
using Authentication.Abstractions.Credentials;
using Employee.GrpcClient;
using IdentityAuthentication.Abstractions;

namespace Authentication.Employee
{
    internal class EmployeeAuthenticationService : IAuthenticationService<PasswordCredential>
    {
        private readonly IEmployeeProvider _employeeProvider;

        public EmployeeAuthenticationService(IEmployeeProvider employeeProvider)
        {
            _employeeProvider = employeeProvider;
        }

        public string GrantSource => "Employee";

        public async Task<IAuthenticationResult> AuthenticateAsync(PasswordCredential credential)
        {
            var r = await _employeeProvider.LoginAsync(credential.Username, credential.Password);
            if (r.Result == null) throw new Exception(r.Error.Message);

            var user = r.Result;
            var metadata = new Dictionary<string, string> {
                {"Email",user.Email },
                {"Gender",user.Gender }
            };
            return credential.CreateAuthenticationResult(user.UserId, metadata, username: user.Username);
        }

        public async Task<bool> IdentityCheckAsync(string id, string username)
        {
            var r = await _employeeProvider.GetEmployeeAsync(id);
            if (r == null) return false;

            return true;
        }
    }
}
