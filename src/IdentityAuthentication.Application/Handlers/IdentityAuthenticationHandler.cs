using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Core;
using IdentityAuthentication.Model.Extensions;
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
        private readonly IAuthenticationProvider _identityAuthenticationProvider;

        public IdentityAuthenticationHandler(
            IOptionsMonitor<IdentityAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAuthenticationProvider identityAuthenticationProvider)
            : base(options, logger, encoder, clock)
        {
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(), IdentityAuthenticationDefaultKeys.AuthenticationScheme);
            EmptyAuthenticateSuccessResult = AuthenticateResult.Success(ticket);
            _identityAuthenticationProvider = identityAuthenticationProvider;
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

            var endpoint = Context.GetEndpoint();
            if (endpoint == null) return AuthenticateResult.NoResult();

            var allowAnonymous = endpoint.Metadata.GetMetadata<IAllowAnonymous>();
            if (allowAnonymous != null) return EmptyAuthenticateSuccessResult;

            var token = messageReceivedContext.Token;
            if (token.IsNullOrEmpty())
            {
                token = Request.Headers.GetAuthorization();
                if (token.IsNullOrEmpty()) return AuthenticateResult.NoResult();
            }

            var tokenValidationResult = await _identityAuthenticationProvider.ValidateTokenAsync(token);
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
