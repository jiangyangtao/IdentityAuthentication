using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.Model.Handlers
{
    public class TokenValidatedContext : ResultContext<IdentityAuthenticationSchemeOptions>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="TokenValidatedContext"/>.
        /// </summary>
        /// <inheritdoc />
        public TokenValidatedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            IdentityAuthenticationSchemeOptions options)
            : base(context, scheme, options) { }

        /// <summary>
        /// Gets or sets the validated security token.
        /// </summary>
        public SecurityToken SecurityToken { get; set; } = default!;
    }
}
