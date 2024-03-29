﻿using IdentityAuthentication.Abstractions;
using IdentityAuthentication.Extensions;
using IdentityAuthentication.Model.Extensions;

namespace Authentication.Abstractions.Credentials
{
    public class PasswordCredential : ICredential
    {
        public string GrantType => "Password";

        public string GrantSource { set; get; }

        public string Client { set; get; }

        public string Username { set; get; }

        public string Password { set; get; }

        public IAuthenticationResult CreateAuthenticationResult(string id, IReadOnlyDictionary<string, string> metadata, string username = "")
        {
            return AuthenticationResult.CreateAuthenticationResult(
                userId: id,
                username: username.NotNullAndEmpty() ? username : Username,
                grantSource: GrantSource,
                grantType: GrantType,
                client: Client,
                metadata: metadata);
        }
    }
}
