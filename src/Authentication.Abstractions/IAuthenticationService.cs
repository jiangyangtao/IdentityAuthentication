﻿

namespace Authentication.Abstractions
{
    public interface IAuthenticationService<in TCredential> where TCredential : ICredential
    {
        string AuthenticationSource { get; }

        Task<AuthenticationResult> AuthenticateAsync(TCredential credential);

        Task<bool> IdentityCheckAsync(string id, string username);
    }
}