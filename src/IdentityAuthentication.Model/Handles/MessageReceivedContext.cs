using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace IdentityAuthentication.Model.Handlers
{
    public class MessageReceivedContext : ResultContext<AuthenticationSchemeOptions>
    {
        public MessageReceivedContext(HttpContext context, AuthenticationScheme scheme, AuthenticationSchemeOptions options) : base(context, scheme, options) { }

        public string? Token { get; set; }
    }
}
