using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;

namespace IdentityAuthentication.Application.Handlers
{
    public class MessageReceivedContext : ResultContext<IdentityAuthenticationSchemeOptions>
    {
        public MessageReceivedContext(HttpContext context, AuthenticationScheme scheme, IdentityAuthenticationSchemeOptions options) : base(context, scheme, options) { }

        public string? Token { get; set; }
    }
}
