using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace IdentityAuthentication.Application.Handlers
{
    public class IdentityAuthenticationHandler : AuthenticationHandler<IdentityAuthenticationSchemeOptions>
    {
        private readonly AuthenticateResult EmptyAuthenticateSuccessResult;
        private readonly IAuthenticationProvider _authenticationProvider;

        public IdentityAuthenticationHandler(
            IOptionsMonitor<IdentityAuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock, 
            IAuthenticationProvider authenticationProvider)
            : base(options, logger, encoder, clock)
        {
            var ticket = new AuthenticationTicket(new System.Security.Claims.ClaimsPrincipal(), IdentityAuthenticationDefaults.AuthenticationScheme);
            EmptyAuthenticateSuccessResult = AuthenticateResult.Success(ticket);
            _authenticationProvider = authenticationProvider;
        }

        protected new IdentityAuthenticationEvents Events
        {
            get => (IdentityAuthenticationEvents)base.Events!;
            set => base.Events = value;
        }


        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var messageReceivedContext = new MessageReceivedContext(Context, Scheme, Options);

            // event can set the token
            await Events.MessageReceived(messageReceivedContext);
            if (messageReceivedContext.Result != null)
            {
                return messageReceivedContext.Result;
            }

            var r = Context.GetEndpoint().Metadata.GetMetadata<IAllowAnonymous>();
            if (r != null) return EmptyAuthenticateSuccessResult;

            var token = messageReceivedContext.Token;
            if (token.IsNullOrEmpty())
            {
                var authorization = Request.Headers.Authorization.ToString();
                if (authorization.IsNullOrEmpty()) return AuthenticateResult.NoResult();

                if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = authorization.Substring("Bearer ".Length).Trim();
                }

                if (token.IsNullOrEmpty()) return AuthenticateResult.NoResult();
            }

            //var r = await _authenticationProvider.AuthenticateAsync();

            return await Context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            return base.HandleChallengeAsync(properties);
        }

        protected override Task InitializeEventsAsync()
        {
            return base.InitializeEventsAsync();
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            return base.HandleForbiddenAsync(properties);
        }

        protected override Task InitializeHandlerAsync()
        {
            return base.InitializeHandlerAsync();
        }

        protected override Task<object> CreateEventsAsync()
        {
            return base.CreateEventsAsync();
        }

        protected override string? ResolveTarget(string? scheme)
        {
            return base.ResolveTarget(scheme);
        }
    }
}
