using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace IdentityAuthentication.Model.Handlers
{
    public class MessageReceivedContext : ResultContext<IdentityAuthenticationSchemeOptions>
    {
        public MessageReceivedContext(HttpContext context, AuthenticationScheme scheme, IdentityAuthenticationSchemeOptions options) : base(context, scheme, options) { }

        public string? Token { get; set; }
    }
}
