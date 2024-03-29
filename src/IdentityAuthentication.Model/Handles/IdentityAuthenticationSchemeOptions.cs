﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAuthentication.Model.Handlers
{
    public class IdentityAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public TokenValidationParameters TokenValidationParameters { get; set; } = new TokenValidationParameters();

        public new IdentityAuthenticationEvents Events
        {
            get { return (IdentityAuthenticationEvents)base.Events!; }
            set { base.Events = value; }
        }
    }
}
