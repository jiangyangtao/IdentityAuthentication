using Authentication.Abstractions;
using Authentication.Abstractions.Credentials;
using FirmAccount.Authentication.GrpcClient;
using IdentityAuthentication.Abstractions;

namespace Authentication.Firm
{
    public class FirmAuthenticationService : IAuthenticationService<PasswordCredential>
    {
        private readonly IUserAuthenticationProvider _userAuthenticationProvider;

        public FirmAuthenticationService(IUserAuthenticationProvider userAuthenticationProvider)
        {
            _userAuthenticationProvider = userAuthenticationProvider;
        }

        public string GrantSource => "Firm";

        public async Task<IAuthenticationResult> AuthenticateAsync(PasswordCredential credential)
        {
            var r = await _userAuthenticationProvider.LoginAsync(credential.Username, credential.Password);
            if (r.Result == null) throw new Exception(r.Error.Message);

            var user = r.Result;
            var metadata = new Dictionary<string, string> {
                {"FirmCode",user.FirmCode },
                {"FirmAccountName",user.FirmAccountName },
                {"UserType",user.UserType.ToString() },
            };
            return credential.CreateAuthenticationResult(user.UserId, metadata, username: user.Username);
        }

        public async Task<bool> IdentityValidationAsync(string id, string username)
        {
            var r = await _userAuthenticationProvider.ValidationAsync(id);
            if (r == null) return false;

            return true;
        }
    }
}
