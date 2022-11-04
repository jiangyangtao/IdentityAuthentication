using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace IdentityAuthentication.Application.Handlers
{
    public class AuthenticationHandler : IAuthenticationHandler
    {
        public IdentityAuthenticationSchemeOptions Options { get; private set; } = default!;

        private HttpContext httpContext;

        protected IOptionsMonitor<IdentityAuthenticationSchemeOptions> OptionsMonitor { get; }

        public AuthenticationHandler(IOptionsMonitor<IdentityAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        {
            OptionsMonitor = options;
        }

        public async Task<AuthenticateResult> AuthenticateAsync()
        {

            return await httpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
        }

        public Task ChallengeAsync(AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        public Task ForbidAsync(AuthenticationProperties? properties)
        {
            throw new NotImplementedException();
        }

        public async Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            Options = OptionsMonitor.Get(scheme.Name);
            httpContext = context;
            await Task.CompletedTask;
        }
    }
}
