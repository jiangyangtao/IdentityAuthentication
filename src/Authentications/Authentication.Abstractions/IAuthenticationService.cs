﻿using IdentityAuthentication.Configuration.Abstractions;

namespace Authentication.Abstractions
{
    public interface IAuthenticationService<in TCredential> where TCredential : ICredential
    {
        string GrantSource { get; }

        Task<IAuthenticationResult> AuthenticateAsync(TCredential credential);

        Task<bool> IdentityCheckAsync(string id, string username);
    }
}