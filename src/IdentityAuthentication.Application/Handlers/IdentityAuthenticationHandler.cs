using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
using IdentityAuthentication.Model.Handlers;
using IdentityAuthentication.Model.Handles;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
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
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(), IdentityAuthenticationDefaults.AuthenticationScheme);
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

            await Events.MessageReceived(messageReceivedContext);
            if (messageReceivedContext.Result != null)
            {
                return messageReceivedContext.Result;
            }

            var allowAnonymous = Context.GetEndpoint().Metadata.GetMetadata<IAllowAnonymous>();
            if (allowAnonymous != null) return EmptyAuthenticateSuccessResult;

            var token = messageReceivedContext.Token;
            if (token.IsNullOrEmpty())
            {
                var authorization = Request.Headers.Authorization.ToString();
                if (authorization.IsNullOrEmpty()) return AuthenticateResult.NoResult();

                if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = authorization["Bearer ".Length..].Trim();
                }

                if (token.IsNullOrEmpty()) return AuthenticateResult.NoResult();
            }

            var tokenValidationResult = await _authenticationProvider.AuthorizeAsync(token);
            if (tokenValidationResult.IsValid == false) return AuthenticateResult.NoResult();

            var principal = tokenValidationResult.ClaimsIdentity.ToClaimsPrincipal();
            var tokenValidatedContext = new TokenValidatedContext(Context, Scheme, Options)
            {
                Principal = principal,
                SecurityToken = tokenValidationResult.SecurityToken
            };

            await Events.OnTokenValidated(tokenValidatedContext);
            if (tokenValidatedContext.Result != null) return tokenValidatedContext.Result;

            tokenValidatedContext.Success();
            return tokenValidatedContext.Result!;
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
